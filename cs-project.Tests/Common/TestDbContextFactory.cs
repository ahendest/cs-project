using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using cs_project.Core.Entities;
using static cs_project.Core.Entities.Enums;
using System.Threading.Tasks;

namespace cs_project.Tests.Common;

public static class TestDbContextFactory
{
    public static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    public static async Task<Station> SeedStationAsync(AppDbContext ctx)
    {
        var station = new Station { Id = 1, Name = "Main", Address = "Addr" };
        await ctx.Stations.AddAsync(station);
        await ctx.SaveChangesAsync();
        return station;
    }

    public static async Task<Tank> SeedTankAsync(AppDbContext ctx)
    {
        var station = await ctx.Stations.FirstOrDefaultAsync() ?? await SeedStationAsync(ctx);
        var tank = new Tank { Id = 1, StationId = station.Id, Station = station, FuelType = FuelType.Diesel };
        await ctx.Tanks.AddAsync(tank);
        await ctx.SaveChangesAsync();
        return tank;
    }
}
