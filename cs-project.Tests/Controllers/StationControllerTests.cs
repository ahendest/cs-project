using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class StationControllerTests
{
    [Fact]
    public async Task GetStation_ReturnsOk_WhenExists()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.GetStationByIdAsync(1)).ReturnsAsync(new StationDTO { Id = 1 });
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.GetStation(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<StationDTO>(ok.Value);
    }

    [Fact]
    public async Task GetStation_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.GetStationByIdAsync(1)).ReturnsAsync((StationDTO?)null);
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.GetStation(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateStation_ReturnsCreated()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.CreateStationAsync(It.IsAny<StationCreateDTO>())).ReturnsAsync(new StationDTO { Id = 1 });
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.CreateStation(new StationCreateDTO { Name = "S", Address = "A" });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<StationDTO>(created.Value);
    }

    [Fact]
    public async Task UpdateStation_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.UpdateStationAsync(1, It.IsAny<StationCreateDTO>())).ReturnsAsync(true);
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.UpdateStation(1, new StationCreateDTO { Name = "S", Address = "A" });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateStation_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.UpdateStationAsync(1, It.IsAny<StationCreateDTO>())).ReturnsAsync(false);
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.UpdateStation(1, new StationCreateDTO { Name = "S", Address = "A" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteStation_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.DeleteStationAsync(1)).ReturnsAsync(true);
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.DeleteStation(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteStation_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IStationService>();
        var logger = new Mock<ILogger<StationController>>();
        service.Setup(s => s.DeleteStationAsync(1)).ReturnsAsync(false);
        var controller = new StationController(service.Object, logger.Object);

        var result = await controller.DeleteStation(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
