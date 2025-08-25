using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class CustomerTransactionControllerTests
{
    [Fact]
    public async Task GetCustomerTransaction_ReturnsOk_WhenExists()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.GetCustomerTransactionByIdAsync(1)).ReturnsAsync(new CustomerTransactionDTO { Id = 1 });
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.GetCustomerTransaction(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<CustomerTransactionDTO>(ok.Value);
    }

    [Fact]
    public async Task GetCustomerTransaction_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.GetCustomerTransactionByIdAsync(1)).ReturnsAsync((CustomerTransactionDTO?)null);
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.GetCustomerTransaction(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCustomerTransaction_ReturnsCreated()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.CreateCustomerTransactionAsync(It.IsAny<CustomerTransactionCreateDTO>())).ReturnsAsync(new CustomerTransactionDTO { Id = 1 });
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.CreateCustomerTransaction(new CustomerTransactionCreateDTO { PumpId = 1, StationFuelPriceId = 1, Liters = 1, PricePerLiter = 1, TotalPrice = 1, TimestampUtc = DateTimeOffset.UtcNow });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<CustomerTransactionDTO>(created.Value);
    }

    [Fact]
    public async Task UpdateCustomerTransaction_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.UpdateCustomerTransactionAsync(1, It.IsAny<CustomerTransactionCreateDTO>())).ReturnsAsync(true);
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.UpdateCustomerTransaction(1, new CustomerTransactionCreateDTO { PumpId = 1, StationFuelPriceId = 1, Liters = 1, PricePerLiter = 1, TotalPrice = 1, TimestampUtc = DateTimeOffset.UtcNow });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCustomerTransaction_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.UpdateCustomerTransactionAsync(1, It.IsAny<CustomerTransactionCreateDTO>())).ReturnsAsync(false);
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.UpdateCustomerTransaction(1, new CustomerTransactionCreateDTO { PumpId = 1, StationFuelPriceId = 1, Liters = 1, PricePerLiter = 1, TotalPrice = 1, TimestampUtc = DateTimeOffset.UtcNow });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCustomerTransaction_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.DeleteCustomerTransactionAsync(1)).ReturnsAsync(true);
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.DeleteCustomerTransaction(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCustomerTransaction_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICustomerTransactionService>();
        var logger = new Mock<ILogger<CustomerTransactionController>>();
        service.Setup(s => s.DeleteCustomerTransactionAsync(1)).ReturnsAsync(false);
        var controller = new CustomerTransactionController(service.Object, logger.Object);

        var result = await controller.DeleteCustomerTransaction(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
