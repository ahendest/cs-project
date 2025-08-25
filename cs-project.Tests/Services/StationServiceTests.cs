using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class StationServiceTests
{
    private readonly IMapper _mapper;
    public StationServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetById_ReturnsDto_WhenFound()
    {
        var repo = new Mock<IStationRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Station { Id = 1, Name="S", Address="A" });
        var service = new StationService(repo.Object, _mapper);

        var result = await service.GetStationByIdAsync(1, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenMissing()
    {
        var repo = new Mock<IStationRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Station?)null);
        var service = new StationService(repo.Object, _mapper);

        var result = await service.GetStationByIdAsync(1, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_CallsRepository_WhenFound()
    {
        var entity = new Station { Id = 1, Name="S", Address="A" };
        var repo = new Mock<IStationRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var service = new StationService(repo.Object, _mapper);

        var result = await service.DeleteStationAsync(1, CancellationToken.None);

        Assert.True(result);
        repo.Verify(r => r.Delete(entity), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
