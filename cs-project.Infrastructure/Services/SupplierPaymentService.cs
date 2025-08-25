using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class SupplierPaymentService : ISupplierPaymentService
    {
        private readonly ISupplierPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public SupplierPaymentService(ISupplierPaymentRepository paymentRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<SupplierPaymentDTO>> GetSupplierPaymentAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _paymentRepository.QuerySupplierPaymentsAsync(query);
            var dtoList = _mapper.Map<IEnumerable<SupplierPaymentDTO>>(entities);

            return new PagedResult<SupplierPaymentDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<SupplierPaymentDTO>> GetAllSupplierPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierPaymentDTO>>(payments);
        }

        public async Task<SupplierPaymentDTO?> GetSupplierPaymentByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<SupplierPaymentDTO>(payment);
        }

        public async Task<SupplierPaymentDTO> CreateSupplierPaymentAsync(SupplierPaymentCreateDTO dto)
        {
            var payment = _mapper.Map<SupplierPayment>(dto);
            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();
            return _mapper.Map<SupplierPaymentDTO>(payment);
        }

        public async Task<bool> UpdateSupplierPaymentAsync(int id, SupplierPaymentCreateDTO dto)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return false;

            _mapper.Map(dto, payment);
            _paymentRepository.Update(payment);
            return await _paymentRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSupplierPaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null) return false;

            _paymentRepository.Delete(payment);
            return await _paymentRepository.SaveChangesAsync();
        }
    }
}
