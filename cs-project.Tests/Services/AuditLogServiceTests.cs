using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities.Audit;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Moq;

namespace cs_project.Tests.Services;

public class AuditLogServiceTests
{
    private readonly IMapper _mapper;

    public AuditLogServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetById_ReturnsDto_WhenFound()
    {
        var repo = new Mock<IAuditLogRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new AuditLog { Id = 1 });
        var service = new AuditLogService(repo.Object, _mapper);

        var result = await service.GetAuditLogByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenMissing()
    {
        var repo = new Mock<IAuditLogRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((AuditLog?)null);
        var service = new AuditLogService(repo.Object, _mapper);

        var result = await service.GetAuditLogByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_CallsRepository_WhenFound()
    {
        var entity = new AuditLog { Id = 1 };
        var repo = new Mock<IAuditLogRepository>();
        repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(true).Verifiable();
        var service = new AuditLogService(repo.Object, _mapper);

        var result = await service.DeleteAuditLogAsync(1);

        Assert.True(result);
        repo.Verify(r => r.Delete(entity), Times.Once);
        repo.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
}
