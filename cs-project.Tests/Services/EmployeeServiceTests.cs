using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;
using System.Threading;

namespace cs_project.Tests.Services;

public class EmployeeServiceTests
{
    private readonly IMapper _mapper;
    public EmployeeServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsDto()
    {
        var repo = new Mock<IEmployeeRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new Employee { Id = 1, FirstName = "John", LastName = "Doe" });
        var service = new EmployeeService(repo.Object, _mapper);

        var result = await service.GetEmployeeByIdAsync(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_CallsRepository()
    {
        var employee = new Employee { Id = 1, FirstName = "John", LastName = "Doe" };
        var repo = new Mock<IEmployeeRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(employee);
        repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).Verifiable();
        var service = new EmployeeService(repo.Object, _mapper);

        var result = await service.DeleteEmployeeAsync(1, CancellationToken.None);

        Assert.True(result);
        repo.Verify(r => r.Delete(employee), Times.Once);
    }
}
