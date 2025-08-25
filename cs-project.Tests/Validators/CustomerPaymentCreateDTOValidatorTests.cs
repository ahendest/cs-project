using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class CustomerPaymentCreateDTOValidatorTests
{
    private readonly CustomerPaymentCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CustomerPaymentCreateDTO
        {
            CustomerTransactionId = 1,
            Method = PaymentMethod.Cash,
            Amount = 10m,
            AuthorizationCode = "AUTH",
            ProcessedAtUtc = DateTime.UtcNow
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new CustomerPaymentCreateDTO
        {
            CustomerTransactionId = 0,
            Method = (PaymentMethod)(-1),
            Amount = -1m,
            AuthorizationCode = new string('a', 101),
            ProcessedAtUtc = default
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "CustomerTransactionId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Method" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Amount" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "AuthorizationCode" && x.ErrorMessage.Contains("characters"));
        Assert.Contains(result.Errors, x => x.PropertyName == "ProcessedAtUtc" && x.ErrorMessage.Contains("not be empty"));
    }
}
