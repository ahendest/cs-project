using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Repositories;

public class PumpRepositoryTests
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
    public async Task AddAndGetPump_ReturnsPump()
    {
        using var ctx = CreateContext();
        var repo = new PumpRepository(ctx);
        var pump = new Pump { Id = 1, Tank = new Tank { FuelType = FuelType.LPG } };

        await repo.AddAsync(pump);
        await repo.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(1);

        Assert.NotNull(fetched);
        Assert.Equal(1, fetched!.Id);
    }
}
