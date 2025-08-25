using cs_project.Core.Entities;
using cs_project.Core.Models;
using System.Threading;

namespace cs_project.Infrastructure.Repositories
{
    public interface ICustomerPaymentRepository
    {
        Task<(IEnumerable<CustomerPayment> Items, int TotalCount)> QueryCustomerPaymentsAsync(PagingQueryParameters query, CancellationToken ct);
        Task<IEnumerable<CustomerPayment>> GetAllAsync(CancellationToken ct);
        Task<CustomerPayment?> GetByIdAsync(int id, CancellationToken ct);
        Task AddAsync(CustomerPayment payment, CancellationToken ct);
        void Update(CustomerPayment payment);
        void Delete(CustomerPayment payment);
        Task<bool> SaveChangesAsync(CancellationToken ct);
    }
}
