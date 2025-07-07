using cs_project.Core.DTOs;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task<PagedResult<TransactionsDTO>> GetTransactionsAsync(PagingQueryParameters query);
        Task<IEnumerable<TransactionsDTO>> GetAllAsync();
        Task<TransactionsDTO?> GetByIdAsync(int id);
        Task<TransactionsDTO> CreateAsync(TransactionsCreateDTO transactionDto);
        Task<bool> UpdateAsync(int id, TransactionsCreateDTO transactionDto);
        Task<bool> DeleteAsync(int id);
    }
}
