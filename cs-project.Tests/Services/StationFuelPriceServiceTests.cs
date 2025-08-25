using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class StationFuelPriceServiceTests
{
    private readonly IMapper _mapper;
    public StationFuelPriceServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetById_ReturnsDto_WhenFound()
    {
        var repo = new Mock<IStationFuelPriceRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new StationFuelPrice { Id = 1 });
        var service = new StationFuelPriceService(repo.Object, _mapper);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenMissing()
    {
        var repo = new Mock<IStationFuelPriceRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((StationFuelPrice?)null);
        var service = new StationFuelPriceService(repo.Object, _mapper);

        var result = await service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_CallsRepository_WhenFound()
    {
        var entity = new StationFuelPrice { Id = 1 };
        var repo = new Mock<IStationFuelPriceRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).Verifiable();
        var service = new StationFuelPriceService(repo.Object, _mapper);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(entity), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
