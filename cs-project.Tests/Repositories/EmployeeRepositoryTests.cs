using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;
using System.Threading;
using System.Threading.Tasks;
using cs_project.Tests.Common;

namespace cs_project.Tests.Repositories;

public class EmployeeRepositoryTests
{
    private static AppDbContext CreateContext() => TestDbContextFactory.CreateContext();

    private static Task<Station> SeedStationAsync(AppDbContext ctx) => TestDbContextFactory.SeedStationAsync(ctx);

    [Fact]
    public async Task AddAndFetchEmployee_Works()
    {
        await using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow,
            StationId = station.Id
        };

        await repo.AddAsync(employee, CancellationToken.None);
        await repo.SaveChangesAsync(CancellationToken.None);

        var fetched = await repo.GetByIdAsync(1, CancellationToken.None);
        Assert.NotNull(fetched);
        Assert.Equal("John", fetched!.FirstName);
    }

    [Fact]
    public async Task UpdateEmployee_PersistsChanges()
    {
        await using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow,
            StationId = station.Id
        };
        await repo.AddAsync(employee, CancellationToken.None);
        await repo.SaveChangesAsync(CancellationToken.None);

        employee.LastName = "Smith";
        repo.Update(employee);
        await repo.SaveChangesAsync(CancellationToken.None);

        var fetched = await repo.GetByIdAsync(1, CancellationToken.None);
        Assert.Equal("Smith", fetched!.LastName);
    }

    [Fact]
    public async Task DeleteEmployee_RemovesEntity()
    {
        await using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow,
            StationId = station.Id
        };
        await repo.AddAsync(employee, CancellationToken.None);
        await repo.SaveChangesAsync(CancellationToken.None);

        repo.Delete(employee);
        await repo.SaveChangesAsync(CancellationToken.None);

        var fetched = await repo.GetByIdAsync(1, CancellationToken.None);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetById_Missing_ReturnsNull()
    {
        await using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var fetched = await repo.GetByIdAsync(999, CancellationToken.None);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddEmployee_MissingNames_Throws()
    {
        await using var ctx = CreateContext();
        var repo = new EmployeeRepository(ctx);
        var employee = new Employee
        {
            Id = 1,
            FirstName = null!,
            LastName = null!,
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow
        };
        await repo.AddAsync(employee, CancellationToken.None);
        await Assert.ThrowsAsync<DbUpdateException>(() => repo.SaveChangesAsync(CancellationToken.None));
    }
}

