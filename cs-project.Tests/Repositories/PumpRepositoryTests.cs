using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using static cs_project.Core.Entities.Enums;
using cs_project.Tests.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using cs_project.Core.Models;

namespace cs_project.Tests.Repositories;

public class PumpRepositoryTests
{
    private static AppDbContext CreateContext() => TestDbContextFactory.CreateContext();

    private static Task<Tank> SeedTankAsync(AppDbContext ctx) => TestDbContextFactory.SeedTankAsync(ctx);

    [Fact]
    public async Task AddAndFetchPump_Works()
    {
        await using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var tank = await SeedTankAsync(ctx);
        var pump = new Pump { Id = 1, TankId = tank.Id, Tank = tank };

        await repo.AddAsync(pump);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);

        Assert.NotNull(fetched);
        Assert.Equal(1, fetched!.Id);
    }

    [Fact]
    public async Task UpdatePump_PersistsChanges()
    {
        await using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var tank = await SeedTankAsync(ctx);
        var pump = new Pump { Id = 1, TankId = tank.Id, Tank = tank, Status = PumpStatus.Idle };
        await repo.AddAsync(pump);
        await repo.SaveChangesAsync();

        pump.Status = PumpStatus.Offline;
        repo.Update(pump);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Equal(PumpStatus.Offline, fetched!.Status);
    }

    [Fact]
    public async Task DeletePump_RemovesEntity()
    {
        await using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var tank = await SeedTankAsync(ctx);
        var pump = new Pump { Id = 1, TankId = tank.Id, Tank = tank };
        await repo.AddAsync(pump);
        await repo.SaveChangesAsync();

        repo.Delete(pump);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetById_Missing_ReturnsNull()
    {
        await using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var fetched = await repo.GetByIdAsync(123);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddPump_DuplicateKey_Throws()
    {
        await using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var tank = await SeedTankAsync(ctx);
        var pump1 = new Pump { Id = 1, TankId = tank.Id, Tank = tank };
        await repo.AddAsync(pump1);
        await repo.SaveChangesAsync();

        var pump2 = new Pump { Id = 1, TankId = tank.Id, Tank = tank };
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repo.AddAsync(pump2);
        });
    }

    [Fact]
    public async Task QueryPumpsAsync_FiltersSortsAndPages()
    {
        await using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var station = await TestDbContextFactory.SeedStationAsync(ctx);
        var tank1 = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.Diesel };
        var tank2 = new Tank { Id = 2, StationId = station.Id, Station = station, FuelType = FuelType.LPG };
        await ctx.Tanks.AddRangeAsync(tank1, tank2);
        await ctx.SaveChangesAsync();

        var pump1 = new Pump { Id = 1, TankId = tank1.Id, Tank = tank1, Status = PumpStatus.Idle };
        var pump2 = new Pump { Id = 2, TankId = tank2.Id, Tank = tank2, Status = PumpStatus.Dispensing };
        var pump3 = new Pump { Id = 3, TankId = tank1.Id, Tank = tank1, Status = PumpStatus.Maintenance };
        await ctx.Pumps.AddRangeAsync(pump1, pump2, pump3);
        await ctx.SaveChangesAsync();

        var query = new PagingQueryParameters
        {
            SearchTerm = "Diesel",
            SortBy = "status_desc",
            Page = 1,
            PageSize = 1
        };

        var (items, total) = await repo.QueryPumpsAsync(query);
        Assert.Equal(2, total);
        var first = Assert.Single(items);
        Assert.Equal(PumpStatus.Maintenance, first.Status);

        query.Page = 2;
        (items, total) = await repo.QueryPumpsAsync(query);
        Assert.Equal(2, total);
        first = Assert.Single(items);
        Assert.Equal(PumpStatus.Idle, first.Status);
    }
}
