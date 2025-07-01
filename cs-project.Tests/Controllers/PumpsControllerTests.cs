using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class PumpsControllerTests
{
    [Fact]
    public async Task GetPump_ReturnsNotFound_WhenNull()
    {
        var service = new Mock<IPumpService>();
        service.Setup(s => s.GetPumpByIdAsync(1)).ReturnsAsync((PumpDTO?)null);
        var controller = new PumpsController(service.Object);

        var result = await controller.GetPump(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeletePump_ReturnsNoContent()
    {
        var service = new Mock<IPumpService>();
        service.Setup(s => s.DeletePumpAsync(1)).ReturnsAsync(true);
        var controller = new PumpsController(service.Object);

        var result = await controller.DeletePump(1);

        Assert.IsType<NoContentResult>(result);
    }
}
