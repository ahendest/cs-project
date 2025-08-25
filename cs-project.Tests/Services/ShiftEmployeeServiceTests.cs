using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class ShiftEmployeeServiceTests
{
    private readonly IMapper _mapper;
    public ShiftEmployeeServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task AddEmployee_ReturnsDto_WhenNew()
    {
        var repo = new Mock<IShiftEmployeeRepository>();
        repo.Setup(r => r.GetByShiftAndEmployeeIdAsync(1, 2)).ReturnsAsync((ShiftEmployee?)null);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
        var service = new ShiftEmployeeService(repo.Object, _mapper);

        var result = await service.AddEmployeeToShiftAsync(1, 2);

        Assert.NotNull(result);
        repo.Verify(r => r.AddAsync(It.Is<ShiftEmployee>(se => se.ShiftId == 1 && se.EmployeeId == 2)), Times.Once);
    }

    [Fact]
    public async Task AddEmployee_ReturnsNull_WhenExists()
    {
        var repo = new Mock<IShiftEmployeeRepository>();
        repo.Setup(r => r.GetByShiftAndEmployeeIdAsync(1, 2)).ReturnsAsync(new ShiftEmployee());
        var service = new ShiftEmployeeService(repo.Object, _mapper);

        var result = await service.AddEmployeeToShiftAsync(1, 2);

        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveEmployee_ReturnsFalse_WhenMissing()
    {
        var repo = new Mock<IShiftEmployeeRepository>();
        repo.Setup(r => r.GetByShiftAndEmployeeIdAsync(1, 2)).ReturnsAsync((ShiftEmployee?)null);
        var service = new ShiftEmployeeService(repo.Object, _mapper);

        var result = await service.RemoveEmployeeFromShiftAsync(1, 2);

        Assert.False(result);
    }
}
