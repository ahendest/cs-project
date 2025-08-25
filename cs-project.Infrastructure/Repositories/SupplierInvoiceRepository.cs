using cs_project.Core.Entities;
using cs_project.Core.Models;
using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Repositories;

public class SupplierInvoiceRepository : ISupplierInvoiceRepository
{
    private readonly AppDbContext _db;

    public SupplierInvoiceRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(IEnumerable<SupplierInvoice> Items, int TotalCount)> QuerySupplierInvoicesAsync(PagingQueryParameters query)
    {
        var invoices = _db.SupplierInvoices.AsNoTracking();

        invoices = query.SortBy?.ToLower() switch
        {
            "deliverydate_desc" => invoices.OrderByDescending(i => i.DeliveryDateUtc),
            "deliverydate" => invoices.OrderBy(i => i.DeliveryDateUtc),
            _ => invoices.OrderBy(i => i.Id)
        };

        int totalCount = await invoices.CountAsync();
        int page = query.Page > 0 ? query.Page : 1;
        int pageSize = query.PageSize > 0 && query.PageSize <= 100 ? query.PageSize : 20;
        invoices = invoices.Skip((page - 1) * pageSize).Take(pageSize);
        var items = await invoices.ToListAsync();
        return (items, totalCount);
    }

    public async Task<IEnumerable<SupplierInvoice>> GetAllAsync() =>
        await _db.SupplierInvoices.AsNoTracking().ToListAsync();

    public async Task<SupplierInvoice?> GetByIdAsync(int id) =>
        await _db.SupplierInvoices.FindAsync(id);

    public async Task AddAsync(SupplierInvoice invoice) =>
        await _db.SupplierInvoices.AddAsync(invoice);

    public void Update(SupplierInvoice invoice) =>
        _db.SupplierInvoices.Update(invoice);

    public void Delete(SupplierInvoice invoice) =>
        _db.SupplierInvoices.Remove(invoice);

    public async Task<bool> SaveChangesAsync() =>
        await _db.SaveChangesAsync() > 0;
}
