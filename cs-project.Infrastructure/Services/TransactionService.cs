using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;
using cs_project.Core.Exceptions;

namespace cs_project.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPumpRepository _pumpRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IPumpRepository pumpRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _pumpRepository = pumpRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<TransactionsDTO>> GetTransactionsAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _transactionRepository.QueryTransactionsAsync(query);
            var dtos = _mapper.Map<IEnumerable<TransactionsDTO>>(entities);

            return new PagedResult<TransactionsDTO>
            {
                Items = dtos,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<TransactionsDTO>> GetAllAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TransactionsDTO>>(transactions);
        }

        public async Task<TransactionsDTO?> GetByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            return transaction == null ? null : _mapper.Map<TransactionsDTO>(transaction);
        }

        public async Task<TransactionsDTO> CreateAsync(TransactionsCreateDTO transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            var pumpId = await _pumpRepository.GetByIdAsync(transactionDto.PumpId);
            if(pumpId == null) throw new NotFoundException("Pump not found");

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return _mapper.Map<TransactionsDTO>(transaction);
        }

        public async Task<bool> UpdateAsync(int id, TransactionsCreateDTO transactionDto)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id);
            if (existingTransaction == null) return false;

            _mapper.Map(transactionDto, existingTransaction);
            _transactionRepository.Update(existingTransaction);
            return await _transactionRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return false;

            _transactionRepository.Delete(transaction);
            
            return await _transactionRepository.SaveChangesAsync();
        }
    }
}
