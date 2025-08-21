using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;

public class SuppliersControllerTests
{
    [Fact]
    public async Task GetSupplier_ReturnsNotFound_WhenNull()
    {
        var service = new Mock<ISupplierService>();
        var logger = new Mock<ILogger<SupplierController>>();
        service.Setup(s => s.GetSupplierByIdAsync(1)).ReturnsAsync((SupplierDTO?)null);
        var controller = new SupplierController(service.Object, logger.Object);

        var result = await controller.GetSupplier(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteSupplier_ReturnsNoContent()
    {
        var service = new Mock<ISupplierService>();
        var logger = new Mock<ILogger<SupplierController>>();
        service.Setup(s => s.DeleteSupplierAsync(1)).ReturnsAsync(true);
        var controller = new SupplierController(service.Object, logger.Object);

        var result = await controller.DeleteSupplier(1);

        Assert.IsType<NoContentResult>(result);
    }
}
