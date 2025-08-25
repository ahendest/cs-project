using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class TankControllerTests
{
    [Fact]
    public async Task GetTank_ReturnsOk_WhenExists()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.GetTankByIdAsync(1)).ReturnsAsync(new TankDTO { Id = 1 });
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.GetTank(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<TankDTO>(ok.Value);
    }

    [Fact]
    public async Task GetTank_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.GetTankByIdAsync(1)).ReturnsAsync((TankDTO?)null);
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.GetTank(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateTank_ReturnsCreated()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.CreateTankAsync(It.IsAny<TankCreateDTO>())).ReturnsAsync(new TankDTO { Id = 1 });
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.CreateTank(new TankCreateDTO { StationId = 1, FuelType = FuelType.Diesel });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<TankDTO>(created.Value);
    }

    [Fact]
    public async Task UpdateTank_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.UpdateTankAsync(1, It.IsAny<TankCreateDTO>())).ReturnsAsync(true);
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.UpdateTank(1, new TankCreateDTO { StationId = 1, FuelType = FuelType.Diesel });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateTank_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.UpdateTankAsync(1, It.IsAny<TankCreateDTO>())).ReturnsAsync(false);
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.UpdateTank(1, new TankCreateDTO { StationId = 1, FuelType = FuelType.Diesel });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteTank_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.DeleteTankAsync(1)).ReturnsAsync(true);
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.DeleteTank(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTank_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ITankService>();
        var logger = new Mock<ILogger<TankController>>();
        service.Setup(s => s.DeleteTankAsync(1)).ReturnsAsync(false);
        var controller = new TankController(service.Object, logger.Object);

        var result = await controller.DeleteTank(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
