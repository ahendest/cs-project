using cs_project.Controllers;
using cs_project.Core.Interfaces;
using cs_project.Core.Entities.Pricing;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class PricingControllerTests
{
    [Fact]
    public async Task Reprice_ReturnsOk()
    {
        var service = new Mock<IPricingService>();
        service.Setup(s => s.RepriceStationAsync(1, null, It.IsAny<CancellationToken>())).ReturnsAsync(new List<StationFuelPrice>());
        var controller = new PricingController(service.Object);

        var result = await controller.Reprice(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Current_ReturnsOk()
    {
        var service = new Mock<IPricingService>();
        service.Setup(s => s.GetCurrentPriceAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new List<StationFuelPrice>());
        var controller = new PricingController(service.Object);

        var result = await controller.Current(1, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }
}
