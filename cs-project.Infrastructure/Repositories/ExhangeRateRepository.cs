using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories
{
    public class ExchangeRateRepository(AppDbContext db) : IExchangeRateRepository
    {
        public async Task<decimal?> GetLatestUsdToRonAsync(CancellationToken ct = default) =>
            await db.ExchangeRates
                .Where(x => x.BaseCurrency == "USD" && x.QuoteCurrency == "RON" && x.IsActive)
                .OrderByDescending(x => x.RetrievedAtUtc)
                .Select(x => (decimal?)x.Rate)
                .FirstOrDefaultAsync(ct);
    }
}
