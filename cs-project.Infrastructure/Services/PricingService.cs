using static cs_project.Core.Entities.Enums;
using cs_project.Core.Interfaces;
using cs_project.Core.Entities.Pricing;

namespace cs_project.Infrastructure.Services
{
    public class PricingService(
        IExchangeRateRepository fxRepo,
        IPricePolicyRepository policyRepo,
        ISupplierCostRepository costRepo,
        IStationFuelPriceRepository priceRepo) : IPricingService
    {
        public async Task<IReadOnlyList<StationFuelPrice>> RepriceStationAsync(int stationId, DateTime? asOfUtc = null, CancellationToken ct = default)
        {
            var now = asOfUtc ?? DateTime.UtcNow;
            var fx = await fxRepo.GetLatestUsdToRonAsync(ct) ?? 4.3m;
            var active = await policyRepo.GetActivePoliciesAsync(stationId, now, ct);

            var selected = active
                .GroupBy(p => p.FuelType)
                .Select(g => g.OrderByDescending(p => p.StationId.HasValue).ThenByDescending(p => p.Priority).First())
                .ToList();

            var results = new List<StationFuelPrice>();

            foreach (var pol in selected)
            {
                var price = await ComputePriceAsync(pol, stationId, fx, now, ct);
                if (price is null) continue;

                await priceRepo.AddAsync(price, ct);
                results.Add(price);
            }

            await priceRepo.SaveChangesAsync(ct);
            return results;
        }

        public Task<IReadOnlyList<StationFuelPrice>> GetCurrentPriceAsync(int stationId, CancellationToken ct = default)
        {
            DateTime now = DateTime.UtcNow;
            return priceRepo.GetCurrentForStationAsync(stationId, now, ct);
        }

        private async Task<StationFuelPrice?> ComputePriceAsync(PricePolicy pol, int stationId, decimal fx, DateTime now, CancellationToken ct)
        {
            var baseUsd = pol.BaseUsdPrice ?? 1.50m;
            var marginPct = pol.MarginPct ?? 0m;
            var marginRon = pol.MarginRon ?? 0m;

            decimal priceRon;
            decimal? costRon = null;

            switch (pol.Method)
            {
                case PricingMethod.FlatUsd:
                    var baseRon = baseUsd * fx;
                    priceRon = ApplyMargins(baseRon, marginPct, marginRon);
                    break;

                case PricingMethod.CostPlus:
                    var since = now.AddDays(-60);
                    var wac = await costRepo.GetWACRonAsync(stationId, pol.FuelType, since, ct);
                    if (wac is null) return null;
                    costRon = wac.Value;
                    priceRon = ApplyMargins(costRon.Value, marginPct, marginRon);
                    break;

                case PricingMethod.Manual:
                    return null;

                default:
                    return null;
            }
            priceRon = ApplyRounding(priceRon, pol.RoundingIncrement, pol.RoundingMode);

            return new StationFuelPrice
            {
                StationId = stationId,
                FuelType = pol.FuelType,
                PriceRon = priceRon,
                EffectiveFromUtc = now,
                DerivedFromPolicyId = pol.Id,
                FxRateUsed = fx,
                CostRonUsed = costRon,
                Reason = $"{pol.Method} publish",
            };
        }

        private static decimal ApplyMargins(decimal baseRon, decimal pct, decimal addRon) =>
            baseRon * (1 + pct) + addRon;

        private static decimal ApplyRounding(decimal value, decimal step, RoundingMode mode)
        {
            if (step <= 0) return value;
            var k = value / step;

            return mode switch
            {
                RoundingMode.Floor => Math.Floor(k) * step,
                RoundingMode.Ceil => Math.Ceiling(k) * step,
                _ => Math.Round(k, MidpointRounding.AwayFromZero) * step
            };
        }
    }
}
