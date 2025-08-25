using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;

public class SupplierInvoicesControllerTests
{
    [Fact]
    public async Task GetInvoice_ReturnsNotFound_WhenNull()
    {
        var service = new Mock<ISupplierInvoiceService>();
        var lineService = new Mock<ISupplierInvoiceLineService>();
        var logger = new Mock<ILogger<SupplierInvoiceController>>();
        service.Setup(s => s.GetSupplierInvoiceByIdAsync(1)).ReturnsAsync((SupplierInvoiceDTO?)null);
        var controller = new SupplierInvoiceController(service.Object, lineService.Object, logger.Object);

        var result = await controller.GetInvoice(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteInvoice_ReturnsNoContent()
    {
        var service = new Mock<ISupplierInvoiceService>();
        var lineService = new Mock<ISupplierInvoiceLineService>();
        var logger = new Mock<ILogger<SupplierInvoiceController>>();
        service.Setup(s => s.DeleteSupplierInvoiceAsync(1)).ReturnsAsync(true);
        var controller = new SupplierInvoiceController(service.Object, lineService.Object, logger.Object);

        var result = await controller.DeleteInvoice(1);

        Assert.IsType<NoContentResult>(result);
    }
}
