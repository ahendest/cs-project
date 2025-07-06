using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;

public class FuelPriceControllerTests
{
    [Fact]
    public async Task GetFuelPrice_ReturnsNotFound_WhenServiceReturnsNull()
    {
        var service = new Mock<IFuelPriceService>();
        var logger = new Mock<ILogger<FuelPriceController>>();
        service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((FuelPriceDTO?)null);
        var controller = new FuelPriceController(service.Object, logger.Object);

        var result = await controller.GetFuelPrice(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateFuelPrice_ReturnsCreated()
    {
        var service = new Mock<IFuelPriceService>();
        var logger = new Mock<ILogger<FuelPriceController>>();
        service.Setup(s => s.CreateAsync(It.IsAny<FuelPriceCreateDTO>())).ReturnsAsync(new FuelPriceDTO { Id = 1 });
        var controller = new FuelPriceController(service.Object, logger.Object);

        var result = await controller.CreateFuelPrice(new FuelPriceCreateDTO());

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }

    [Fact]
    public async Task GetFuelPrice_ReturnsOk_WhenFound()
    {
        var service = new Mock<IFuelPriceService>();
        var logger = new Mock<ILogger<FuelPriceController>>();
        service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new FuelPriceDTO { Id = 1 });
        var controller = new FuelPriceController(service.Object, logger.Object);

        var result = await controller.GetFuelPrice(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<FuelPriceDTO>(okResult.Value);
        Assert.Equal(1, dto.Id);
    }

    [Fact]
    public async Task CreateFuelPrice_ReturnsBadRequest_WhenModelInvalid()
    {
        var service = new Mock<IFuelPriceService>();
        var logger = new Mock<ILogger<FuelPriceController>>();
        var controller = new FuelPriceController(service.Object, logger.Object);
        controller.ModelState.AddModelError("FuelType", "Required");

        var result = await controller.CreateFuelPrice(new FuelPriceCreateDTO());

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
