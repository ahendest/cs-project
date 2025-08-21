using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ICustomerPaymentService
    {
        Task<PagedResult<CustomerPaymentDTO>> GetCustomerPaymentAsync(PagingQueryParameters query);
        Task<IEnumerable<CustomerPaymentDTO>> GetAllCustomerPaymentsAsync();
        Task<CustomerPaymentDTO?> GetCustomerPaymentByIdAsync(int id);
        Task<CustomerPaymentDTO> CreateCustomerPaymentAsync(CustomerPaymentCreateDTO dto);
        Task<bool> UpdateCustomerPaymentAsync(int id, CustomerPaymentCreateDTO dto);
        Task<bool> DeleteCustomerPaymentAsync(int id);
    }
}
