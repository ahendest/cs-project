using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class StationFuelPriceControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk_WhenExists()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new StationFuelPriceDTO { Id = 1 });
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Get(1, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<StationFuelPriceDTO>(ok.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((StationFuelPriceDTO?)null);
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Get(1, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.CreateAsync(It.IsAny<StationFuelPriceCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(new StationFuelPriceDTO { Id = 1 });
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Create(new StationFuelPriceCreateDTO { StationId = 1, FuelType = FuelType.Diesel, PriceRon = 1, EffectiveFromUtc = DateTime.UtcNow }, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<StationFuelPriceDTO>(created.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.UpdateAsync(1, It.IsAny<StationFuelPriceCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Update(1, new StationFuelPriceCreateDTO { StationId = 1, FuelType = FuelType.Diesel, PriceRon = 1, EffectiveFromUtc = DateTime.UtcNow }, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.UpdateAsync(1, It.IsAny<StationFuelPriceCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Update(1, new StationFuelPriceCreateDTO { StationId = 1, FuelType = FuelType.Diesel, PriceRon = 1, EffectiveFromUtc = DateTime.UtcNow }, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IStationFuelPriceService>();
        service.Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new StationFuelPriceController(service.Object);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
