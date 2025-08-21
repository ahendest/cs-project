using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class CustomerTransactionService : ICustomerTransactionService
    {
        private readonly ICustomerTransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public CustomerTransactionService(ICustomerTransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CustomerTransactionDTO>> GetCustomerTransactionAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _transactionRepository.QueryCustomerTransactionsAsync(query);
            var dtoList = _mapper.Map<IEnumerable<CustomerTransactionDTO>>(entities);

            return new PagedResult<CustomerTransactionDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<CustomerTransactionDTO>> GetAllCustomerTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerTransactionDTO>>(transactions);
        }

        public async Task<CustomerTransactionDTO?> GetCustomerTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            return transaction == null ? null : _mapper.Map<CustomerTransactionDTO>(transaction);
        }

        public async Task<CustomerTransactionDTO> CreateCustomerTransactionAsync(CustomerTransactionCreateDTO dto)
        {
            var transaction = _mapper.Map<CustomerTransaction>(dto);
            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
            return _mapper.Map<CustomerTransactionDTO>(transaction);
        }

        public async Task<bool> UpdateCustomerTransactionAsync(int id, CustomerTransactionCreateDTO dto)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return false;

            _mapper.Map(dto, transaction);
            _transactionRepository.Update(transaction);
            return await _transactionRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCustomerTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return false;

            _transactionRepository.Delete(transaction);
            return await _transactionRepository.SaveChangesAsync();
        }
    }
}
