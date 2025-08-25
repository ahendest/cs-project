using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Pricing;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class ExchangeRateServiceTests
{
    private readonly IMapper _mapper;
    public ExchangeRateServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetById_ReturnsDto_WhenFound()
    {
        var repo = new Mock<IExchangeRateRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(new ExchangeRate { Id = 1, BaseCurrency="USD", QuoteCurrency="RON" });
        var service = new ExchangeRateService(repo.Object, _mapper);

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenMissing()
    {
        var repo = new Mock<IExchangeRateRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((ExchangeRate?)null);
        var service = new ExchangeRateService(repo.Object, _mapper);
        var result = await service.GetByIdAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_CallsRepository_WhenFound()
    {
        var entity = new ExchangeRate { Id = 1, BaseCurrency="USD", QuoteCurrency="RON" };
        var repo = new Mock<IExchangeRateRepository>();
        repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        repo.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).Verifiable();
        var service = new ExchangeRateService(repo.Object, _mapper);

        var result = await service.DeleteAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(entity), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
