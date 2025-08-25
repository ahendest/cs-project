using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace cs_project.Tests.Services;

public class SupplierPaymentApplyServiceTests
{
    private readonly IMapper _mapper;
    public SupplierPaymentApplyServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateApply_Succeeds_WhenValid()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("apply_success").Options;
        using var db = new AppDbContext(options);
        db.SupplierPayments.Add(new SupplierPayment { Id = 1, SupplierId = 1, Amount = 100, Applies = new List<SupplierPaymentApply>() });
        db.SupplierInvoices.Add(new SupplierInvoice { Id = 1, SupplierId = 1, Lines = new List<SupplierInvoiceLine>{ new(){ QuantityLiters = 100, UnitPrice = 1 } }, Applies = new List<SupplierPaymentApply>() });
        await db.SaveChangesAsync();

        var repo = new Mock<ISupplierPaymentApplyRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<SupplierPaymentApply>())).Returns(Task.CompletedTask);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true);
        var service = new SupplierPaymentApplyService(repo.Object, db, _mapper);

        var dto = new SupplierPaymentApplyCreateDTO { SupplierPaymentId = 1, SupplierInvoiceId = 1, AppliedAmount = 50 };
        var result = await service.CreateSupplierPaymentApplyAsync(dto);

        Assert.NotNull(result);
        repo.Verify(r => r.AddAsync(It.IsAny<SupplierPaymentApply>()), Times.Once);
    }

    [Fact]
    public async Task CreateApply_Throws_WhenSupplierMismatch()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("apply_fail").Options;
        using var db = new AppDbContext(options);
        db.SupplierPayments.Add(new SupplierPayment { Id = 1, SupplierId = 1, Amount = 100, Applies = new List<SupplierPaymentApply>() });
        db.SupplierInvoices.Add(new SupplierInvoice { Id = 1, SupplierId = 2, Lines = new List<SupplierInvoiceLine>{ new(){ QuantityLiters = 100, UnitPrice = 1 } }, Applies = new List<SupplierPaymentApply>() });
        await db.SaveChangesAsync();

        var repo = new Mock<ISupplierPaymentApplyRepository>();
        var service = new SupplierPaymentApplyService(repo.Object, db, _mapper);
        var dto = new SupplierPaymentApplyCreateDTO { SupplierPaymentId = 1, SupplierInvoiceId = 1, AppliedAmount = 50 };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateSupplierPaymentApplyAsync(dto));
    }
}
