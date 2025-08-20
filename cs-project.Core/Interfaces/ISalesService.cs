using cs_project.Core.Entities;

namespace cs_project.Core.Interfaces
{
    public interface ISalesService
    {
        Task<CustomerTransaction> CreateSaleAsync(int pumpId, decimal liters, CancellationToken ct);
    }
}
