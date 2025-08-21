using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ICustomerTransactionService
    {
        Task<PagedResult<CustomerTransactionDTO>> GetCustomerTransactionAsync(PagingQueryParameters query);
        Task<IEnumerable<CustomerTransactionDTO>> GetAllCustomerTransactionsAsync();
        Task<CustomerTransactionDTO?> GetCustomerTransactionByIdAsync(int id);
        Task<CustomerTransactionDTO> CreateCustomerTransactionAsync(CustomerTransactionCreateDTO dto);
        Task<bool> UpdateCustomerTransactionAsync(int id, CustomerTransactionCreateDTO dto);
        Task<bool> DeleteCustomerTransactionAsync(int id);
    }
}
