using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Repositories;
using cs_project.Core.Models;

namespace cs_project.Infrastructure.Services;

public class SupplierInvoiceLineService : ISupplierInvoiceLineService
{
    private readonly ISupplierInvoiceLineRepository _lineRepository;
    private readonly ISupplierInvoiceRepository _invoiceRepository;
    private readonly ITankRepository _tankRepository;
    private readonly IMapper _mapper;

    public SupplierInvoiceLineService(ISupplierInvoiceLineRepository lineRepository,
        ISupplierInvoiceRepository invoiceRepository,
        ITankRepository tankRepository,
        IMapper mapper)
    {
        _lineRepository = lineRepository;
        _invoiceRepository = invoiceRepository;
        _tankRepository = tankRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<SupplierInvoiceLineDTO>> GetSupplierInvoiceLineAsync(PagingQueryParameters query)
    {
        var (entities, total) = await _lineRepository.QuerySupplierInvoiceLinesAsync(query);
        var dtoList = _mapper.Map<IEnumerable<SupplierInvoiceLineDTO>>(entities);
        return new PagedResult<SupplierInvoiceLineDTO>
        {
            Items = dtoList,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<IEnumerable<SupplierInvoiceLineDTO>> GetAllSupplierInvoiceLinesAsync()
    {
        var lines = await _lineRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierInvoiceLineDTO>>(lines);
    }

    public async Task<SupplierInvoiceLineDTO?> GetSupplierInvoiceLineByIdAsync(int id)
    {
        var line = await _lineRepository.GetByIdAsync(id);
        return line == null ? null : _mapper.Map<SupplierInvoiceLineDTO>(line);
    }

    public async Task<IEnumerable<SupplierInvoiceLineDTO>> GetLinesByInvoiceIdAsync(int invoiceId)
    {
        var lines = await _lineRepository.GetByInvoiceIdAsync(invoiceId);
        return _mapper.Map<IEnumerable<SupplierInvoiceLineDTO>>(lines);
    }

    public async Task<SupplierInvoiceLineDTO> CreateSupplierInvoiceLineAsync(SupplierInvoiceLineCreateDTO dto)
    {
        if (await _invoiceRepository.GetByIdAsync(dto.SupplierInvoiceId) == null)
            throw new KeyNotFoundException("SupplierInvoice not found");
        if (await _tankRepository.GetByIdAsync(dto.TankId) == null)
            throw new KeyNotFoundException("Tank not found");

        var line = _mapper.Map<SupplierInvoiceLine>(dto);
        await _lineRepository.AddAsync(line);
        await _lineRepository.SaveChangesAsync();
        return _mapper.Map<SupplierInvoiceLineDTO>(line);
    }

    public async Task<bool> UpdateSupplierInvoiceLineAsync(int id, SupplierInvoiceLineCreateDTO dto)
    {
        var line = await _lineRepository.GetByIdAsync(id);
        if (line == null) return false;
        if (await _invoiceRepository.GetByIdAsync(dto.SupplierInvoiceId) == null) return false;
        if (await _tankRepository.GetByIdAsync(dto.TankId) == null) return false;

        _mapper.Map(dto, line);
        _lineRepository.Update(line);
        return await _lineRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteSupplierInvoiceLineAsync(int id)
    {
        var line = await _lineRepository.GetByIdAsync(id);
        if (line == null) return false;
        _lineRepository.Delete(line);
        return await _lineRepository.SaveChangesAsync();
    }
}
