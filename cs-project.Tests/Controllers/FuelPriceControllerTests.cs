using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class FuelPriceControllerTests
{
    [Fact]
    public async Task GetFuelPrice_ReturnsNotFound_WhenServiceReturnsNull()
    {
        var service = new Mock<IFuelPriceService>();
        service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((FuelPriceDTO?)null);
        var controller = new FuelPriceController(service.Object);

        var result = await controller.GetFuelPrice(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateFuelPrice_ReturnsCreated()
    {
        var service = new Mock<IFuelPriceService>();
        service.Setup(s => s.CreateAsync(It.IsAny<FuelPriceCreateDTO>())).ReturnsAsync(new FuelPriceDTO { Id = 1 });
        var controller = new FuelPriceController(service.Object);

        var result = await controller.CreateFuelPrice(new FuelPriceCreateDTO());

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}
