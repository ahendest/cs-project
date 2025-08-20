namespace cs_project.Core.Interfaces
{
    public interface IExchangeRateRepository
    {
        Task<decimal?> GetLatestUsdToRonAsync(CancellationToken ct = default);
    }
}
