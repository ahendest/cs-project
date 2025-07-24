using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Services;

public class FuelPriceServiceTests
{
    private readonly IMapper _mapper;
    public FuelPriceServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllDtos()
    {
        var prices = new List<FuelPrice> {
        new FuelPrice { Id = 1, FuelType = FuelType.Diesel, CurrentPrice = 1.0 },
        new FuelPrice { Id = 2, FuelType = FuelType.LPG, CurrentPrice = 2.0 }
        };
        var repo = new Mock<IFuelPriceRepository>();
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(prices);
        var service = new FuelPriceService(repo.Object, _mapper);

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        var repo = new Mock<IFuelPriceRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((FuelPrice?)null);
        var service = new FuelPriceService(repo.Object, _mapper);

        var result = await service.GetByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CallsRepository()
    {
        var repo = new Mock<IFuelPriceRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<FuelPrice>())).Returns(Task.CompletedTask).Verifiable();
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var service = new FuelPriceService(repo.Object, _mapper);
        var dto = new FuelPriceCreateDTO { FuelType = "A", CurrentPrice = 1.2, UpdatedAt = DateTime.Now };

        var result = await service.CreateAsync(dto);

        repo.Verify();
        Assert.Equal("A", result.FuelType);
    }

    [Fact]
    public async Task CreateAsync_Throws_WhenRepositoryFails()
    {
        var repo = new Mock<IFuelPriceRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<FuelPrice>())).ThrowsAsync(new InvalidOperationException());
        var service = new FuelPriceService(repo.Object, _mapper);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(new FuelPriceCreateDTO()));
    }
}
