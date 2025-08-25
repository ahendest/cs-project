using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Repositories;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services;

public class SupplierInvoiceService : ISupplierInvoiceService
{
    private readonly ISupplierInvoiceRepository _invoiceRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IStationRepository _stationRepository;
    private readonly IMapper _mapper;

    public SupplierInvoiceService(ISupplierInvoiceRepository invoiceRepository,
        ISupplierRepository supplierRepository,
        IStationRepository stationRepository,
        IMapper mapper)
    {
        _invoiceRepository = invoiceRepository;
        _supplierRepository = supplierRepository;
        _stationRepository = stationRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<SupplierInvoiceDTO>> GetSupplierInvoiceAsync(PagingQueryParameters query)
    {
        var (entities, total) = await _invoiceRepository.QuerySupplierInvoicesAsync(query);
        var dtoList = _mapper.Map<IEnumerable<SupplierInvoiceDTO>>(entities);
        return new PagedResult<SupplierInvoiceDTO>
        {
            Items = dtoList,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<IEnumerable<SupplierInvoiceDTO>> GetAllSupplierInvoicesAsync()
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierInvoiceDTO>>(invoices);
    }

    public async Task<SupplierInvoiceDTO?> GetSupplierInvoiceByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        return invoice == null ? null : _mapper.Map<SupplierInvoiceDTO>(invoice);
    }

    public async Task<SupplierInvoiceDTO> CreateSupplierInvoiceAsync(SupplierInvoiceCreateDTO dto)
    {
        if (await _supplierRepository.GetByIdAsync(dto.SupplierId) == null)
            throw new KeyNotFoundException("Supplier not found");
        if (await _stationRepository.GetByIdAsync(dto.StationId) == null)
            throw new KeyNotFoundException("Station not found");

        var invoice = _mapper.Map<SupplierInvoice>(dto);
        await _invoiceRepository.AddAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        return _mapper.Map<SupplierInvoiceDTO>(invoice);
    }

    public async Task<bool> UpdateSupplierInvoiceAsync(int id, SupplierInvoiceCreateDTO dto)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null) return false;
        if (await _supplierRepository.GetByIdAsync(dto.SupplierId) == null) return false;
        if (await _stationRepository.GetByIdAsync(dto.StationId) == null) return false;

        _mapper.Map(dto, invoice);
        _invoiceRepository.Update(invoice);
        return await _invoiceRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteSupplierInvoiceAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null) return false;
        _invoiceRepository.Delete(invoice);
        return await _invoiceRepository.SaveChangesAsync();
    }
}
