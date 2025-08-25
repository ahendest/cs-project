using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class SupplierInvoiceServiceTests
{
    private readonly IMapper _mapper;
    public SupplierInvoiceServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetInvoiceById_ReturnsDto()
    {
        var repo = new Mock<ISupplierInvoiceRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new SupplierInvoice { Id = 1, SupplierId = 1, StationId = 1, DeliveryDateUtc = DateTime.UtcNow });
        var supplierRepo = new Mock<ISupplierRepository>();
        var stationRepo = new Mock<IStationRepository>();
        var service = new SupplierInvoiceService(repo.Object, supplierRepo.Object, stationRepo.Object, _mapper);

        var result = await service.GetSupplierInvoiceByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task DeleteInvoiceAsync_CallsRepository()
    {
        var invoice = new SupplierInvoice { Id = 1, SupplierId = 1, StationId = 1, DeliveryDateUtc = DateTime.UtcNow };
        var repo = new Mock<ISupplierInvoiceRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(invoice);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var supplierRepo = new Mock<ISupplierRepository>();
        var stationRepo = new Mock<IStationRepository>();
        var service = new SupplierInvoiceService(repo.Object, supplierRepo.Object, stationRepo.Object, _mapper);

        var result = await service.DeleteSupplierInvoiceAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(invoice), Times.Once);
    }
}
