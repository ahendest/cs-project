using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Tests.Repositories;

public class SupplierRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    [Fact]
    public async Task AddAndFetchSupplier_Works()
    {
        using var ctx = CreateContext();
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
        using var ctx = CreateContext();
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
        using var ctx = CreateContext();
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
        using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var fetched = await repo.GetByIdAsync(123);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddSupplier_MissingCompanyName_Throws()
    {
        using var ctx = CreateContext();
        var repo = new SupplierRepository(ctx);
        var supplier = new Supplier { Id = 1, CompanyName = null! };
        await repo.AddAsync(supplier);
        await Assert.ThrowsAsync<DbUpdateException>(repo.SaveChangesAsync);
    }
}

