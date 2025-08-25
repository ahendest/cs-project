using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class CustomerPaymentControllerTests
{
    [Fact]
    public async Task GetCustomerPayment_ReturnsOk_WhenExists()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.GetCustomerPaymentByIdAsync(1)).ReturnsAsync(new CustomerPaymentDTO { Id = 1 });
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.GetCustomerPayment(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<CustomerPaymentDTO>(ok.Value);
    }

    [Fact]
    public async Task GetCustomerPayment_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.GetCustomerPaymentByIdAsync(1)).ReturnsAsync((CustomerPaymentDTO?)null);
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.GetCustomerPayment(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCustomerPayment_ReturnsCreated()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.CreateCustomerPaymentAsync(It.IsAny<CustomerPaymentCreateDTO>())).ReturnsAsync(new CustomerPaymentDTO { Id = 1 });
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.CreateCustomerPayment(new CustomerPaymentCreateDTO { CustomerTransactionId = 1, Method = PaymentMethod.Cash, Amount = 1 });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<CustomerPaymentDTO>(created.Value);
    }

    [Fact]
    public async Task UpdateCustomerPayment_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.UpdateCustomerPaymentAsync(1, It.IsAny<CustomerPaymentCreateDTO>())).ReturnsAsync(true);
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.UpdateCustomerPayment(1, new CustomerPaymentCreateDTO { CustomerTransactionId = 1, Method = PaymentMethod.Cash, Amount = 1 });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCustomerPayment_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.UpdateCustomerPaymentAsync(1, It.IsAny<CustomerPaymentCreateDTO>())).ReturnsAsync(false);
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.UpdateCustomerPayment(1, new CustomerPaymentCreateDTO { CustomerTransactionId = 1, Method = PaymentMethod.Cash, Amount = 1 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteCustomerPayment_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.DeleteCustomerPaymentAsync(1)).ReturnsAsync(true);
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.DeleteCustomerPayment(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCustomerPayment_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICustomerPaymentService>();
        var logger = new Mock<ILogger<CustomerPaymentController>>();
        service.Setup(s => s.DeleteCustomerPaymentAsync(1)).ReturnsAsync(false);
        var controller = new CustomerPaymentController(service.Object, logger.Object);

        var result = await controller.DeleteCustomerPayment(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
