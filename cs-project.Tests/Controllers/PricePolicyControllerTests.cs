using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Moq;
using Xunit;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Controllers;

public class PricePolicyControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk_WhenExists()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new PricePolicyDTO { Id = 1 });
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Get(1, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<PricePolicyDTO>(ok.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((PricePolicyDTO?)null);
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Get(1, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreated()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.CreateAsync(It.IsAny<PricePolicyCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PricePolicyDTO { Id = 1 });
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Create(new PricePolicyCreateDTO { FuelType = FuelType.Diesel, Method = PricingMethod.FlatUsd, RoundingMode = RoundingMode.Round, EffectiveFromUtc = DateTime.UtcNow }, CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.IsType<PricePolicyDTO>(created.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdated()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.UpdateAsync(1, It.IsAny<PricePolicyCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Update(1, new PricePolicyCreateDTO { FuelType = FuelType.Diesel, Method = PricingMethod.FlatUsd, RoundingMode = RoundingMode.Round, EffectiveFromUtc = DateTime.UtcNow }, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.UpdateAsync(1, It.IsAny<PricePolicyCreateDTO>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Update(1, new PricePolicyCreateDTO { FuelType = FuelType.Diesel, Method = PricingMethod.FlatUsd, RoundingMode = RoundingMode.Round, EffectiveFromUtc = DateTime.UtcNow }, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IPricePolicyService>();
        service.Setup(s => s.DeleteAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var controller = new PricePolicyController(service.Object);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
