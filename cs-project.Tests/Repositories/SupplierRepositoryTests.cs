using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using cs_project.Tests.Common;

namespace cs_project.Tests.Repositories;

public class SupplierRepositoryTests
{
    private static AppDbContext CreateContext() => TestDbContextFactory.CreateContext();

    [Fact]
    public async Task AddAndFetchSupplier_Works()
    {
        await using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var supplier = new Supplier { Id = 1, CompanyName = "Acme" };

        await repo.AddAsync(supplier);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.NotNull(fetched);
        Assert.Equal("Acme", fetched!.CompanyName);
    }

    [Fact]
    public async Task UpdateSupplier_PersistsChanges()
    {
        await using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var supplier = new Supplier { Id = 1, CompanyName = "Acme" };
        await repo.AddAsync(supplier);
        await repo.SaveChangesAsync();

        supplier.CompanyName = "Updated";
        repo.Update(supplier);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Equal("Updated", fetched!.CompanyName);
    }

    [Fact]
    public async Task DeleteSupplier_RemovesEntity()
    {
        await using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var supplier = new Supplier { Id = 1, CompanyName = "Acme" };
        await repo.AddAsync(supplier);
        await repo.SaveChangesAsync();

        repo.Delete(supplier);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetById_Missing_ReturnsNull()
    {
        await using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var fetched = await repo.GetByIdAsync(123);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddSupplier_MissingCompanyName_Throws()
    {
        await using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var supplier = new Supplier { Id = 1, CompanyName = null! };
        await repo.AddAsync(supplier);
        await Assert.ThrowsAsync<DbUpdateException>(repo.SaveChangesAsync);
    }
}

