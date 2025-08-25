using cs_project.Core.Entities.Pricing;
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

        public async Task<IEnumerable<ExchangeRate>> GetAllAsync(CancellationToken ct = default) =>
            await db.ExchangeRates.AsNoTracking().ToListAsync(ct);

        public async Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await db.ExchangeRates.FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task AddAsync(ExchangeRate rate, CancellationToken ct = default)
        {
            await db.ExchangeRates.AddAsync(rate, ct);
        }

        public void Update(ExchangeRate rate) => db.ExchangeRates.Update(rate);

        public void Delete(ExchangeRate rate) => db.ExchangeRates.Remove(rate);

        public async Task<bool> SaveChangesAsync(CancellationToken ct = default) =>
            await db.SaveChangesAsync(ct) > 0;
    }
}

