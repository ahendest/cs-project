using cs_project.Core.DTOs;

namespace cs_project.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionsDTO>> GetAllAsync();
        Task<TransactionsDTO?> GetByIdAsync(int id);
        Task<TransactionsDTO> CreateAsync(TransactionsCreateDTO transactionDto);
        Task<TransactionsDTO?> UpdateAsync(int id, TransactionsCreateDTO transactionDto);
        Task<bool> DeleteAsync(int id);
    }
}
