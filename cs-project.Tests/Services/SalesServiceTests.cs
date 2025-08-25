using cs_project.Core.Entities;
using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Services;

public class SalesServiceTests
{
    [Fact]
    public async Task CreateSale_CreatesTransaction_WhenValid()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("sales_success").Options;
        using var db = new AppDbContext(options);
        db.Stations.Add(new Station { Id = 1, Name = "S", Address = "A" });
        db.Tanks.Add(new Tank { Id = 1, StationId = 1, FuelType = FuelType.Diesel });
        db.Pumps.Add(new Pump { Id = 1, TankId = 1 });
        await db.SaveChangesAsync();

        var priceRepo = new Mock<IStationFuelPriceRepository>();
        priceRepo.Setup(r => r.GetCurrentAsync(1, FuelType.Diesel, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StationFuelPrice { Id = 1, PriceRon = 5m });

        var pumpRepo = new PumpRepository(db);
        var tankRepo = new TankRepository(db);
        var trxRepo = new CustomerTransactionRepository(db);
        var service = new SalesService(trxRepo, pumpRepo, tankRepo, priceRepo.Object);
        var trx = await service.CreateSaleAsync(1, 10m, CancellationToken.None);

        Assert.Equal(10m, trx.Liters);
        Assert.Equal(1, await db.CustomerTransactions.CountAsync());
    }

    [Fact]
    public async Task CreateSale_Throws_WhenInvalidLiters()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("sales_fail").Options;
        using var db = new AppDbContext(options);
        var priceRepo = new Mock<IStationFuelPriceRepository>();
        var pumpRepo = new PumpRepository(db);
        var tankRepo = new TankRepository(db);
        var trxRepo = new CustomerTransactionRepository(db);
        var service = new SalesService(trxRepo, pumpRepo, tankRepo, priceRepo.Object);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CreateSaleAsync(1, 0m, CancellationToken.None));
    }

    [Fact]
    public async Task GetUpdateDelete_Works()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("sales_ops").Options;
        using var db = new AppDbContext(options);
        db.Stations.Add(new Station { Id = 1, Name = "S", Address = "A" });
        db.Tanks.Add(new Tank { Id = 1, StationId = 1, FuelType = FuelType.Diesel });
        db.Pumps.Add(new Pump { Id = 1, TankId = 1 });
        await db.SaveChangesAsync();

        var priceRepo = new Mock<IStationFuelPriceRepository>();
        priceRepo.Setup(r => r.GetCurrentAsync(1, FuelType.Diesel, It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StationFuelPrice { Id = 1, PriceRon = 5m });

        var pumpRepo = new PumpRepository(db);
        var tankRepo = new TankRepository(db);
        var trxRepo = new CustomerTransactionRepository(db);
        var service = new SalesService(trxRepo, pumpRepo, tankRepo, priceRepo.Object);
        var created = await service.CreateSaleAsync(1, 10m, CancellationToken.None);

        var list = await service.GetSalesAsync(CancellationToken.None);
        Assert.Single(list);

        var fetched = await service.GetSaleByIdAsync(created.Id, CancellationToken.None);
        Assert.NotNull(fetched);

        var updated = await service.UpdateSaleAsync(created.Id, 1, 20m, CancellationToken.None);
        Assert.True(updated);
        Assert.Equal(20m, (await service.GetSaleByIdAsync(created.Id, CancellationToken.None))!.Liters);

        var deleted = await service.DeleteSaleAsync(created.Id, CancellationToken.None);
        Assert.True(deleted);
        Assert.Empty(await service.GetSalesAsync(CancellationToken.None));
    }
}
