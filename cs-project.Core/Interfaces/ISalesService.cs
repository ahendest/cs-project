using cs_project.Core.Entities;

namespace cs_project.Core.Interfaces
{
    public interface ISalesService
    {
        Task<CustomerTransaction> CreateSaleAsync(int pumpId, decimal liters, CancellationToken ct);
        Task<IEnumerable<CustomerTransaction>> GetSalesAsync(CancellationToken ct);
        Task<CustomerTransaction?> GetSaleByIdAsync(int id, CancellationToken ct);
        Task<bool> UpdateSaleAsync(int id, int pumpId, decimal liters, CancellationToken ct);
        Task<bool> DeleteSaleAsync(int id, CancellationToken ct);
    }
}
