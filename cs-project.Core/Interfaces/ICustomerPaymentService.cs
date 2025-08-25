using cs_project.Core.DTOs;
using cs_project.Core.Models;
using System.Threading;

namespace cs_project.Infrastructure.Services
{
    public interface ICustomerPaymentService
    {
        Task<PagedResult<CustomerPaymentDTO>> GetCustomerPaymentAsync(PagingQueryParameters query, CancellationToken ct);
        Task<IEnumerable<CustomerPaymentDTO>> GetAllCustomerPaymentsAsync(CancellationToken ct);
        Task<CustomerPaymentDTO?> GetCustomerPaymentByIdAsync(int id, CancellationToken ct);
        Task<CustomerPaymentDTO> CreateCustomerPaymentAsync(CustomerPaymentCreateDTO dto, CancellationToken ct);
        Task<bool> UpdateCustomerPaymentAsync(int id, CustomerPaymentCreateDTO dto, CancellationToken ct);
        Task<bool> DeleteCustomerPaymentAsync(int id, CancellationToken ct);
    }
}
