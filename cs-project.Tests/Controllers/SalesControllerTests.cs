using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class SalesControllerTests
{
    [Fact]
    public async Task CreateSale_ReturnsOk_WhenValid()
    {
        var service = new Mock<ISalesService>();
        service.Setup(s => s.CreateSaleAsync(1, 1, It.IsAny<CancellationToken>())).ReturnsAsync(new CustomerTransaction());
        var controller = new SalesController(service.Object);

        var result = await controller.CreateSale(new CreateSaleDTO { PumpId = 1, Liters = 1 }, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateSale_ReturnsValidationProblem_WhenInvalid()
    {
        var service = new Mock<ISalesService>();
        var controller = new SalesController(service.Object);
        controller.ModelState.AddModelError("PumpId", "Required");

        var result = await controller.CreateSale(new CreateSaleDTO(), CancellationToken.None);

        var obj = Assert.IsType<ObjectResult>(result);
        Assert.IsType<ValidationProblemDetails>(obj.Value);
    }

    [Fact]
    public async Task GetSales_ReturnsOk()
    {
        var service = new Mock<ISalesService>();
        service.Setup(s => s.GetSalesAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(new List<CustomerTransaction>());
        var controller = new SalesController(service.Object);

        var result = await controller.GetSales(CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetSale_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ISalesService>();
        service.Setup(s => s.GetSaleByIdAsync(1, It.IsAny<CancellationToken>()))
               .ReturnsAsync((CustomerTransaction?)null);
        var controller = new SalesController(service.Object);

        var result = await controller.GetSale(1, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateSale_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<ISalesService>();
        service.Setup(s => s.UpdateSaleAsync(1, 1, 1, It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);
        var controller = new SalesController(service.Object);

        var result = await controller.UpdateSale(1, new CreateSaleDTO { PumpId = 1, Liters = 1 }, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteSale_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<ISalesService>();
        service.Setup(s => s.DeleteSaleAsync(1, It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);
        var controller = new SalesController(service.Object);

        var result = await controller.DeleteSale(1, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }
}
