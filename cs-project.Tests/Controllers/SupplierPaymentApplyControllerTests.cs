using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class SupplierPaymentApplyControllerTests
{
    [Fact]
    public async Task GetSupplierPaymentApply_ReturnsOk_WhenExists()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.GetSupplierPaymentApplyByIdAsync(1)).ReturnsAsync(new SupplierPaymentApplyDTO { Id = 1 });
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.GetSupplierPaymentApply(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<SupplierPaymentApplyDTO>(ok.Value);
    }

    [Fact]
    public async Task GetSupplierPaymentApply_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.GetSupplierPaymentApplyByIdAsync(1)).ReturnsAsync((SupplierPaymentApplyDTO?)null);
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.GetSupplierPaymentApply(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateSupplierPaymentApply_ReturnsCreated()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.CreateSupplierPaymentApplyAsync(It.IsAny<SupplierPaymentApplyCreateDTO>())).ReturnsAsync(new SupplierPaymentApplyDTO { Id = 1 });
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.CreateSupplierPaymentApply(new SupplierPaymentApplyCreateDTO { SupplierPaymentId = 1, SupplierInvoiceId = 1, AppliedAmount = 1 });

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<SupplierPaymentApplyDTO>(created.Value);
    }

    [Fact]
    public async Task UpdateSupplierPaymentApply_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.UpdateSupplierPaymentApplyAsync(1, It.IsAny<SupplierPaymentApplyCreateDTO>())).ReturnsAsync(true);
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.UpdateSupplierPaymentApply(1, new SupplierPaymentApplyCreateDTO { SupplierPaymentId = 1, SupplierInvoiceId = 1, AppliedAmount = 1 });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateSupplierPaymentApply_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.UpdateSupplierPaymentApplyAsync(1, It.IsAny<SupplierPaymentApplyCreateDTO>())).ReturnsAsync(false);
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.UpdateSupplierPaymentApply(1, new SupplierPaymentApplyCreateDTO { SupplierPaymentId = 1, SupplierInvoiceId = 1, AppliedAmount = 1 });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteSupplierPaymentApply_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.DeleteSupplierPaymentApplyAsync(1)).ReturnsAsync(true);
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.DeleteSupplierPaymentApply(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteSupplierPaymentApply_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISupplierPaymentApplyService>();
        var logger = new Mock<ILogger<SupplierPaymentApplyController>>();
        service.Setup(s => s.DeleteSupplierPaymentApplyAsync(1)).ReturnsAsync(false);
        var controller = new SupplierPaymentApplyController(service.Object, logger.Object);

        var result = await controller.DeleteSupplierPaymentApply(1);

        Assert.IsType<NotFoundResult>(result);
    }
}
