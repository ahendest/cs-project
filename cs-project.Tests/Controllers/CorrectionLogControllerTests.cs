using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;
public class CorrectionLogControllerTests
{
    [Fact]
    public async Task GetAllCorrectionLogs_ReturnsOk()
    {
        var service = new Mock<ICorrectionLogService>();
        service.Setup(s => s.GetAllCorrectionLogsAsync()).ReturnsAsync(new List<CorrectionLogDTO> { new CorrectionLogDTO { Id = 1 } });
        var logger = new Mock<ILogger<CorrectionLogController>>();
        var controller = new CorrectionLogController(service.Object, logger.Object);

        var result = await controller.GetAllCorrectionLogs();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var items = Assert.IsAssignableFrom<IEnumerable<CorrectionLogDTO>>(okResult.Value);
        Assert.Single(items);
    }

    [Fact]
    public async Task GetCorrectionLog_ReturnsOk_WhenExists()
    {
        var service = new Mock<ICorrectionLogService>();
        service.Setup(s => s.GetCorrectionLogByIdAsync(1)).ReturnsAsync(new CorrectionLogDTO { Id = 1 });
        var logger = new Mock<ILogger<CorrectionLogController>>();
        var controller = new CorrectionLogController(service.Object, logger.Object);

        var result = await controller.GetCorrectionLog(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<CorrectionLogDTO>(ok.Value);
    }

    [Fact]
    public async Task GetCorrectionLog_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<ICorrectionLogService>();
        service.Setup(s => s.GetCorrectionLogByIdAsync(1)).ReturnsAsync((CorrectionLogDTO?)null);
        var logger = new Mock<ILogger<CorrectionLogController>>();
        var controller = new CorrectionLogController(service.Object, logger.Object);

        var result = await controller.GetCorrectionLog(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
