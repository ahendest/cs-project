using cs_project.Controllers;
using cs_project.Core.DTOs;
using cs_project.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace cs_project.Tests.Controllers;

public class TransactionControllerTests
{
    [Fact]
    public async Task UpdateTransaction_ReturnsNotFound_WhenFalse()
    {
        var service = new Mock<ITransactionService>();
        service.Setup(s => s.UpdateAsync(1, It.IsAny<TransactionsCreateDTO>())).ReturnsAsync(false);
        var controller = new TransactionController(service.Object);

        var result = await controller.UpdateTransaction(1, new TransactionsCreateDTO());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateTransaction_ReturnsCreated()
    {
        var service = new Mock<ITransactionService>();
        service.Setup(s => s.CreateAsync(It.IsAny<TransactionsCreateDTO>())).ReturnsAsync(new TransactionsDTO { Id = 1 });
        var controller = new TransactionController(service.Object);

        var result = await controller.CreateTransaction(new TransactionsCreateDTO());

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}
