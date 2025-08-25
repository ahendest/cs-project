using cs_project.Core.Entities.Pricing;

namespace cs_project.Core.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task<decimal?> GetLatestUsdToRonAsync(CancellationToken ct = default);

        Task<IEnumerable<ExchangeRate>> GetAllAsync(CancellationToken ct = default);
        Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(ExchangeRate rate, CancellationToken ct = default);
        void Update(ExchangeRate rate);
        void Delete(ExchangeRate rate);
        Task<bool> SaveChangesAsync(CancellationToken ct = default);
    }
}

