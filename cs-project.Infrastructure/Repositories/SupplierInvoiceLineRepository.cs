using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories;

public class SupplierInvoiceLineRepository : ISupplierInvoiceLineRepository
{
    private readonly AppDbContext _db;

    public SupplierInvoiceLineRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(IEnumerable<SupplierInvoiceLine> Items, int TotalCount)> QuerySupplierInvoiceLinesAsync(PagingQueryParameters query)
    {
        var lines = _db.SupplierInvoiceLines.AsNoTracking();

        lines = query.SortBy?.ToLower() switch
        {
            "fueltype_desc" => lines.OrderByDescending(l => l.FuelType),
            "fueltype" => lines.OrderBy(l => l.FuelType),
            _ => lines.OrderBy(l => l.Id)
        };

        int totalCount = await lines.CountAsync();
        int page = query.Page > 0 ? query.Page : 1;
        int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
        lines = lines.Skip((page - 1) * pageSize).Take(pageSize);
        var items = await lines.ToListAsync();
        return (items, totalCount);
    }

    public async Task<IEnumerable<SupplierInvoiceLine>> GetAllAsync() =>
        await _db.SupplierInvoiceLines.AsNoTracking().ToListAsync();

    public async Task<SupplierInvoiceLine?> GetByIdAsync(int id) =>
        await _db.SupplierInvoiceLines.FindAsync(id);

    public async Task<IEnumerable<SupplierInvoiceLine>> GetByInvoiceIdAsync(int invoiceId) =>
        await _db.SupplierInvoiceLines.Where(l => l.SupplierInvoiceId == invoiceId).AsNoTracking().ToListAsync();

    public async Task AddAsync(SupplierInvoiceLine line) =>
        await _db.SupplierInvoiceLines.AddAsync(line);

    public void Update(SupplierInvoiceLine line) =>
        _db.SupplierInvoiceLines.Update(line);

    public void Delete(SupplierInvoiceLine line) =>
        _db.SupplierInvoiceLines.Remove(line);

    public async Task<bool> SaveChangesAsync() =>
        await _db.SaveChangesAsync() > 0;
}
