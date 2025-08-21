using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class SupplierServiceTests
{
    private readonly IMapper _mapper;
    public SupplierServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetSupplierById_ReturnsDto()
    {
        var repo = new Mock<ISupplierRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Supplier { Id = 1, CompanyName = "ABC" });
        var service = new SupplierService(repo.Object, _mapper);

        var result = await service.GetSupplierByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task DeleteSupplierAsync_CallsRepository()
    {
        var supplier = new Supplier { Id = 1, CompanyName = "ABC" };
        var repo = new Mock<ISupplierRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(supplier);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var service = new SupplierService(repo.Object, _mapper);

        var result = await service.DeleteSupplierAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(supplier), Times.Once);
    }
}
