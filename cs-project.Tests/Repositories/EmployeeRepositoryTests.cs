using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Repositories;

public class EmployeeRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    private static Station SeedStation(AppDbContext ctx)
    {
        var station = new Station { Id = 1, Name = "Main", Address = "Addr" };
        ctx.Stations.Add(station);
        ctx.SaveChanges();
        return station;
    }

    [Fact]
    public async Task AddAndFetchEmployee_Works()
    {
        using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var station = SeedStation(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow,
            StationId = station.Id
        };

        await repo.AddAsync(employee);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.NotNull(fetched);
        Assert.Equal("John", fetched!.FirstName);
    }

    [Fact]
    public async Task UpdateEmployee_PersistsChanges()
    {
        using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var station = SeedStation(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow,
            StationId = station.Id
        };
        await repo.AddAsync(employee);
        await repo.SaveChangesAsync();

        employee.LastName = "Smith";
        repo.Update(employee);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Equal("Smith", fetched!.LastName);
    }

    [Fact]
    public async Task DeleteEmployee_RemovesEntity()
    {
        using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var station = SeedStation(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow,
            StationId = station.Id
        };
        await repo.AddAsync(employee);
        await repo.SaveChangesAsync();

        repo.Delete(employee);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetById_Missing_ReturnsNull()
    {
        using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var fetched = await repo.GetByIdAsync(999);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddEmployee_MissingNames_Throws()
    {
        using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = null!,
            LastName = null!,
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow
        };
        await repo.AddAsync(employee);
        await Assert.ThrowsAsync<DbUpdateException>(repo.SaveChangesAsync);
    }
}

