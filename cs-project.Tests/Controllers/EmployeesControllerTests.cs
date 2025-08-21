using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;

public class EmployeesControllerTests
{
    [Fact]
    public async Task GetEmployee_ReturnsNotFound_WhenNull()
    {
        var service = new Mock<IEmployeeService>();
        var logger = new Mock<ILogger<EmployeeController>>();
        service.Setup(s => s.GetEmployeeByIdAsync(1)).ReturnsAsync((EmployeeDTO?)null);
        var controller = new EmployeeController(service.Object, logger.Object);

        var result = await controller.GetEmployee(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteEmployee_ReturnsNoContent()
    {
        var service = new Mock<IEmployeeService>();
        var logger = new Mock<ILogger<EmployeeController>>();
        service.Setup(s => s.DeleteEmployeeAsync(1)).ReturnsAsync(true);
        var controller = new EmployeeController(service.Object, logger.Object);

        var result = await controller.DeleteEmployee(1);

        Assert.IsType<NoContentResult>(result);
    }
}
