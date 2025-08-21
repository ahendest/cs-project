using cs_project.Core.Entities;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Repositories
{
    public interface ISupplierPaymentApplyRepository
    {
        Task<(IEnumerable<SupplierPaymentApply> Items, int TotalCount)> QuerySupplierPaymentAppliesAsync(PagingQueryParameters query);
        Task<IEnumerable<SupplierPaymentApply>> GetAllAsync();
        Task<SupplierPaymentApply?> GetByIdAsync(int id);
        Task AddAsync(SupplierPaymentApply apply);
        void Update(SupplierPaymentApply apply);
        void Delete(SupplierPaymentApply apply);
        Task<bool> SaveChangesAsync();
    }
}
