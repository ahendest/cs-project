using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ISupplierPaymentRepository
    {
        Task<(IEnumerable<SupplierPayment> Items, int TotalCount)> QuerySupplierPaymentsAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierPayment>> GetAllAsync();
        Task<SupplierPayment?> GetByIdAsync(int id);
        Task AddAsync(SupplierPayment payment);
        void Update(SupplierPayment payment);
        void Delete(SupplierPayment payment);
        Task<bool> SaveChangesAsync();
    }
}
