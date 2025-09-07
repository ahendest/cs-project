using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;
using cs_project.Tests.Common;
using System.Threading.Tasks;

namespace cs_project.Tests.Repositories;

public class TankRepositoryTests
{
    private static AppDbContext CreateContext() => TestDbContextFactory.CreateContext();

    private static Task<Station> SeedStationAsync(AppDbContext ctx) => TestDbContextFactory.SeedStationAsync(ctx);

    [Fact]
    public async Task AddAndFetchTank_Works()
    {
        await using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var tank = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.Diesel };

        await repo.AddAsync(tank);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.NotNull(fetched);
        Assert.Equal(FuelType.Diesel, fetched!.FuelType);
    }

    [Fact]
    public async Task UpdateTank_PersistsChanges()
    {
        await using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var tank = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.Diesel };
        await repo.AddAsync(tank);
        await repo.SaveChangesAsync();

        tank.FuelType = FuelType.LPG;
        repo.Update(tank);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Equal(FuelType.LPG, fetched!.FuelType);
    }

    [Fact]
    public async Task DeleteTank_RemovesEntity()
    {
        await using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var tank = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.Diesel };
        await repo.AddAsync(tank);
        await repo.SaveChangesAsync();

        repo.Delete(tank);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetById_Missing_ReturnsNull()
    {
        await using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var fetched = await repo.GetByIdAsync(123);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddTank_DuplicateKey_Throws()
    {
        await using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = await SeedStationAsync(ctx);
        var tank1 = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.Diesel };
        await repo.AddAsync(tank1);
        await repo.SaveChangesAsync();

        var tank2 = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.LPG };
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repo.AddAsync(tank2);
        });
    }
}

