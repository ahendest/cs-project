using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
}
