using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class PumpServiceTests
{
    private readonly IMapper _mapper;
    public PumpServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetPumpById_ReturnsDto()
    {
        var repo = new Mock<IPumpRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Pump { Id = 1, FuelType = "A" });
        var service = new PumpService(repo.Object, _mapper);

        var result = await service.GetPumpByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task DeletePumpAsync_CallsRepository()
    {
        var pump = new Pump { Id = 1 };
        var repo = new Mock<IPumpRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(pump);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var service = new PumpService(repo.Object, _mapper);

        var result = await service.DeletePumpAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(pump), Times.Once);
    }
}
