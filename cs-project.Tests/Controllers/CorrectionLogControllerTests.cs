using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class CorrectionLogControllerTests
{
    [Fact]
    public async Task GetCorrectionLog_ReturnsOk_WhenExists()
    {
        var service = new Mock<ICorrectionLogService>();
        service.Setup(s => s.GetCorrectionLogByIdAsync(1)).ReturnsAsync(new CorrectionLogDTO { Id = 1 });
        var controller = new CorrectionLogController(service.Object);

        var result = await controller.GetCorrectionLog(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<CorrectionLogDTO>(ok.Value);
    }

    [Fact]
    public async Task GetCorrectionLog_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICorrectionLogService>();
        service.Setup(s => s.GetCorrectionLogByIdAsync(1)).ReturnsAsync((CorrectionLogDTO?)null);
        var controller = new CorrectionLogController(service.Object);

        var result = await controller.GetCorrectionLog(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
