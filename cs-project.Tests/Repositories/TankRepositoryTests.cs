using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Repositories;

public class TankRepositoryTests
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
    public async Task AddAndFetchTank_Works()
    {
        using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = SeedStation(ctx);
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
        using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = SeedStation(ctx);
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
        using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = SeedStation(ctx);
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
        using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var fetched = await repo.GetByIdAsync(123);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddTank_DuplicateKey_Throws()
    {
        using var ctx = CreateContext();
        var repo = new TankRepository(ctx);
        var station = SeedStation(ctx);
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

