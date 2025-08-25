using AutoMapper;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;
using static cs_project.Core.Entities.Enums;

namespace cs_project.Tests.Services;

public class SupplierInvoiceLineServiceTests
{
    private readonly IMapper _mapper;
    public SupplierInvoiceLineServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetLineById_ReturnsDto()
    {
        var repo = new Mock<ISupplierInvoiceLineRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new SupplierInvoiceLine { Id = 1, SupplierInvoiceId = 1, TankId = 1, FuelType = FuelType.Diesel });
        var invoiceRepo = new Mock<ISupplierInvoiceRepository>();
        var tankRepo = new Mock<ITankRepository>();
        var service = new SupplierInvoiceLineService(repo.Object, invoiceRepo.Object, tankRepo.Object, _mapper);

        var result = await service.GetSupplierInvoiceLineByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task DeleteLineAsync_CallsRepository()
    {
        var line = new SupplierInvoiceLine { Id = 1, SupplierInvoiceId = 1, TankId = 1, FuelType = FuelType.Diesel };
        var repo = new Mock<ISupplierInvoiceLineRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(line);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var invoiceRepo = new Mock<ISupplierInvoiceRepository>();
        var tankRepo = new Mock<ITankRepository>();
        var service = new SupplierInvoiceLineService(repo.Object, invoiceRepo.Object, tankRepo.Object, _mapper);

        var result = await service.DeleteSupplierInvoiceLineAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(line), Times.Once);
    }
}
