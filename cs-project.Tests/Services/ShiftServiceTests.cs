using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class ShiftServiceTests
{
    private readonly IMapper _mapper;
    public ShiftServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetById_ReturnsDto_WhenFound()
    {
        var repo = new Mock<IShiftRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Shift { Id = 1 });
        var service = new ShiftService(repo.Object, _mapper);

        var result = await service.GetShiftByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenMissing()
    {
        var repo = new Mock<IShiftRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Shift?)null);
        var service = new ShiftService(repo.Object, _mapper);

        var result = await service.GetShiftByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_CallsRepository_WhenFound()
    {
        var entity = new Shift { Id = 1 };
        var repo = new Mock<IShiftRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var service = new ShiftService(repo.Object, _mapper);

        var result = await service.DeleteShiftAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(entity), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
