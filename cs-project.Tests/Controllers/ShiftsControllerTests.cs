using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace cs_project.Tests.Controllers;

public class ShiftsControllerTests
{
    [Fact]
    public async Task GetShiftEmployees_ReturnsOk()
    {
        var shiftService = new Mock<IShiftService>();
        var shiftEmpService = new Mock<IShiftEmployeeService>();
        var logger = new Mock<ILogger<ShiftController>>();
        shiftEmpService.Setup(s => s.GetEmployeesByShiftIdAsync(1))
            .ReturnsAsync(new List<ShiftEmployeeDTO>());
        var controller = new ShiftController(shiftService.Object, shiftEmpService.Object, logger.Object);

        var result = await controller.GetShiftEmployees(1);

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task AddShiftEmployee_ReturnsConflict_WhenDuplicate()
    {
        var shiftService = new Mock<IShiftService>();
        var shiftEmpService = new Mock<IShiftEmployeeService>();
        var logger = new Mock<ILogger<ShiftController>>();
        shiftEmpService.Setup(s => s.AddEmployeeToShiftAsync(1, 1))
            .ReturnsAsync((ShiftEmployeeDTO?)null);
        var controller = new ShiftController(shiftService.Object, shiftEmpService.Object, logger.Object);

        var result = await controller.AddShiftEmployee(1, new ShiftEmployeeCreateDTO { ShiftId = 1, EmployeeId = 1 });

        Assert.IsType<ConflictResult>(result.Result);
    }

    [Fact]
    public async Task RemoveShiftEmployee_ReturnsNoContent_WhenRemoved()
    {
        var shiftService = new Mock<IShiftService>();
        var shiftEmpService = new Mock<IShiftEmployeeService>();
        var logger = new Mock<ILogger<ShiftController>>();
        shiftEmpService.Setup(s => s.RemoveEmployeeFromShiftAsync(1, 2))
            .ReturnsAsync(true);
        var controller = new ShiftController(shiftService.Object, shiftEmpService.Object, logger.Object);

        var result = await controller.RemoveShiftEmployee(1, 2);

        Assert.IsType<NoContentResult>(result);
    }
}

