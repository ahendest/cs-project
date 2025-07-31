using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Services;

public class TransactionServiceTests
{
    private readonly IMapper _mapper;
    public TransactionServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
    {
        var repo = new Mock<ITransactionRepository>();
        var pumpRepo = new Mock<IPumpRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Transaction?)null);
        var service = new TransactionService(repo.Object, pumpRepo.Object, _mapper);

        var result = await service.UpdateAsync(1, new TransactionsCreateDTO());

        Assert.False(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsDto()
    {
        var repo = new Mock<ITransactionRepository>();
        var pumpRepo = new Mock<IPumpRepository>();
        
        pumpRepo.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Pump{ Id = 1, TankId = 1, FuelType = FuelType.Diesel, CurrentVolume = 100, Status = PumpStatus.Idle });

        repo.Setup(r => r.AddAsync(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
        var service = new TransactionService(repo.Object, pumpRepo.Object, _mapper);
        var dto = new TransactionsCreateDTO { PumpId = 1, Liters = 10, PricePerLiter = 1, TotalPrice = 10, Timestamp = DateTime.UtcNow };

        var result = await service.CreateAsync(dto);

        Assert.Equal(10, result.TotalPrice);
    }
}
