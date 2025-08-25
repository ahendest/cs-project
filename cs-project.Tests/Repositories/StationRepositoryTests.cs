using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Tests.Repositories;

public class StationRepositoryTests
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
    public async Task AddAndFetchStation_Works()
    {
        using var ctx = CreateContext();
        var repo = new StationRepository(ctx);
        var station = new Station { Id = 1, Name = "Main", Address = "Addr" };

        await repo.AddAsync(station);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.NotNull(fetched);
        Assert.Equal("Main", fetched!.Name);
    }

    [Fact]
    public async Task UpdateStation_PersistsChanges()
    {
        using var ctx = CreateContext();
        var repo = new StationRepository(ctx);
        var station = new Station { Id = 1, Name = "Main", Address = "Addr" };
        await repo.AddAsync(station);
        await repo.SaveChangesAsync();

        station.Name = "Updated";
        repo.Update(station);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Equal("Updated", fetched!.Name);
    }

    [Fact]
    public async Task DeleteStation_RemovesEntity()
    {
        using var ctx = CreateContext();
        var repo = new StationRepository(ctx);
        var station = new Station { Id = 1, Name = "Main", Address = "Addr" };
        await repo.AddAsync(station);
        await repo.SaveChangesAsync();

        repo.Delete(station);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task GetById_Missing_ReturnsNull()
    {
        using var ctx = CreateContext();
        var repo = new StationRepository(ctx);
        var fetched = await repo.GetByIdAsync(123);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task AddStation_MissingName_Throws()
    {
        using var ctx = CreateContext();
        var repo = new StationRepository(ctx);
        var station = new Station { Id = 1, Name = null!, Address = "Addr" };
        await repo.AddAsync(station);
        await Assert.ThrowsAsync<DbUpdateException>(repo.SaveChangesAsync);
    }
}

