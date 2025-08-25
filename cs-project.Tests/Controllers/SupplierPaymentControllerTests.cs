using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class SupplierPaymentControllerTests
{
    [Fact]
    public async Task GetSupplierPayment_ReturnsOk_WhenExists()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.GetSupplierPaymentByIdAsync(1)).ReturnsAsync(new SupplierPaymentDTO { Id = 1 });
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.GetSupplierPayment(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<SupplierPaymentDTO>(ok.Value);
    }

    [Fact]
    public async Task GetSupplierPayment_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.GetSupplierPaymentByIdAsync(1)).ReturnsAsync((SupplierPaymentDTO?)null);
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.GetSupplierPayment(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateSupplierPayment_ReturnsCreated()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.CreateSupplierPaymentAsync(It.IsAny<SupplierPaymentCreateDTO>())).ReturnsAsync(new SupplierPaymentDTO { Id = 1 });
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.CreateSupplierPayment(new SupplierPaymentCreateDTO { SupplierId = 1, Method = PaymentMethod.Cash, Amount = 1 });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<SupplierPaymentDTO>(created.Value);
    }

    [Fact]
    public async Task UpdateSupplierPayment_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.UpdateSupplierPaymentAsync(1, It.IsAny<SupplierPaymentCreateDTO>())).ReturnsAsync(true);
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.UpdateSupplierPayment(1, new SupplierPaymentCreateDTO { SupplierId = 1, Method = PaymentMethod.Cash, Amount = 1 });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateSupplierPayment_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.UpdateSupplierPaymentAsync(1, It.IsAny<SupplierPaymentCreateDTO>())).ReturnsAsync(false);
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.UpdateSupplierPayment(1, new SupplierPaymentCreateDTO { SupplierId = 1, Method = PaymentMethod.Cash, Amount = 1 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteSupplierPayment_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.DeleteSupplierPaymentAsync(1)).ReturnsAsync(true);
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.DeleteSupplierPayment(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteSupplierPayment_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISupplierPaymentService>();
        var logger = new Mock<ILogger<SupplierPaymentController>>();
        service.Setup(s => s.DeleteSupplierPaymentAsync(1)).ReturnsAsync(false);
        var controller = new SupplierPaymentController(service.Object, logger.Object);

        var result = await controller.DeleteSupplierPayment(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
