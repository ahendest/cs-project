using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Repositories;

namespace cs_project.Infrastructure.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<SupplierDTO>> GetSupplierAsync(PagingQueryParameters query)
        {
            var (entities, total) = await _supplierRepository.QuerySuppliersAsync(query);
            var dtoList = _mapper.Map<IEnumerable<SupplierDTO>>(entities);

            return new PagedResult<SupplierDTO>
            {
                Items = dtoList,
                TotalCount = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }

        public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            return supplier == null ? null : _mapper.Map<SupplierDTO>(supplier);
        }

        public async Task<SupplierDTO> CreateSupplierAsync(SupplierCreateDTO dto)
        {
            var supplier = _mapper.Map<Supplier>(dto);
            await _supplierRepository.AddAsync(supplier);
            await _supplierRepository.SaveChangesAsync();
            return _mapper.Map<SupplierDTO>(supplier);
        }

        public async Task<bool> UpdateSupplierAsync(int id, SupplierCreateDTO dto)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return false;

            _mapper.Map(dto, supplier);
            _supplierRepository.Update(supplier);
            return await _supplierRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null) return false;

            _supplierRepository.Delete(supplier);
            return await _supplierRepository.SaveChangesAsync();
        }
    }
}
