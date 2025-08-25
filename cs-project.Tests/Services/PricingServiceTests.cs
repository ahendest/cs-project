using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Services;
using Moq;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Services;

public class PricingServiceTests
{
    [Fact]
    public async Task RepriceStation_AddsPrices_WhenPolicyGeneratesPrice()
    {
        var fxRepo = new Mock<IExchangeRateRepository>();
        var policyRepo = new Mock<IPricePolicyRepository>();
        var costRepo = new Mock<ISupplierCostRepository>();
        var priceRepo = new Mock<IStationFuelPriceRepository>();

        fxRepo.Setup(r => r.GetLatestUsdToRonAsync(It.IsAny<CancellationToken>())).ReturnsAsync(5m);
        policyRepo.Setup(r => r.GetActivePoliciesAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PricePolicy> {
                new() { Id = 1, FuelType = FuelType.Diesel, Method = PricingMethod.FlatUsd, BaseUsdPrice = 1m, MarginPct = 0m }
            });
        priceRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var service = new PricingService(fxRepo.Object, policyRepo.Object, costRepo.Object, priceRepo.Object);

        var result = await service.RepriceStationAsync(1);

        Assert.Single(result);
        priceRepo.Verify(r => r.AddAsync(It.IsAny<StationFuelPrice>(), It.IsAny<CancellationToken>()), Times.Once);
        priceRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RepriceStation_SkipsPolicy_WhenPriceNotComputed()
    {
        var fxRepo = new Mock<IExchangeRateRepository>();
        var policyRepo = new Mock<IPricePolicyRepository>();
        var costRepo = new Mock<ISupplierCostRepository>();
        var priceRepo = new Mock<IStationFuelPriceRepository>();

        fxRepo.Setup(r => r.GetLatestUsdToRonAsync(It.IsAny<CancellationToken>())).ReturnsAsync(5m);
        policyRepo.Setup(r => r.GetActivePoliciesAsync(1, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PricePolicy> {
                new() { Id = 1, FuelType = FuelType.Diesel, Method = PricingMethod.CostPlus, MarginPct = 0m }
            });
        costRepo.Setup(r => r.GetWACRonAsync(1, FuelType.Diesel, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((decimal?)null);
        priceRepo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var service = new PricingService(fxRepo.Object, policyRepo.Object, costRepo.Object, priceRepo.Object);

        var result = await service.RepriceStationAsync(1);

        Assert.Empty(result);
        priceRepo.Verify(r => r.AddAsync(It.IsAny<StationFuelPrice>(), It.IsAny<CancellationToken>()), Times.Never);
        priceRepo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
