using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class AuditLogControllerTests
{
    [Fact]
    public async Task GetAuditLog_ReturnsOk_WhenExists()
    {
        var service = new Mock<IAuditLogService>();
        service.Setup(s => s.GetAuditLogByIdAsync(1)).ReturnsAsync(new AuditLogDTO { Id = 1 });
        var controller = new AuditLogController(service.Object);

        var result = await controller.GetAuditLog(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<AuditLogDTO>(ok.Value);
    }

    [Fact]
    public async Task GetAuditLog_ReturnsNotFound_WhenMissing()
    {
        var service = new Mock<IAuditLogService>();
        service.Setup(s => s.GetAuditLogByIdAsync(1)).ReturnsAsync((AuditLogDTO?)null);
        var controller = new AuditLogController(service.Object);

        var result = await controller.GetAuditLog(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }
}
