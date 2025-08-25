using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;

public class SupplierInvoiceLinesControllerTests
{
    [Fact]
    public async Task GetLine_ReturnsNotFound_WhenNull()
    {
        var service = new Mock<ISupplierInvoiceLineService>();
        var logger = new Mock<ILogger<SupplierInvoiceLineController>>();
        service.Setup(s => s.GetSupplierInvoiceLineByIdAsync(1)).ReturnsAsync((SupplierInvoiceLineDTO?)null);
        var controller = new SupplierInvoiceLineController(service.Object, logger.Object);

        var result = await controller.GetLine(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteLine_ReturnsNoContent()
    {
        var service = new Mock<ISupplierInvoiceLineService>();
        var logger = new Mock<ILogger<SupplierInvoiceLineController>>();
        service.Setup(s => s.DeleteSupplierInvoiceLineAsync(1)).ReturnsAsync(true);
        var controller = new SupplierInvoiceLineController(service.Object, logger.Object);

        var result = await controller.DeleteLine(1);

        Assert.IsType<NoContentResult>(result);
    }
}
