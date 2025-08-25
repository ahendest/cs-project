using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class SupplierPaymentCreateDTOValidatorTests
{
    private readonly SupplierPaymentCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new SupplierPaymentCreateDTO
        {
            SupplierId = 1,
            Method = PaymentMethod.Cash,
            Amount = 100m,
            PaidAtUtc = DateTime.UtcNow
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new SupplierPaymentCreateDTO
        {
            SupplierId = 0,
            Method = (PaymentMethod)(-1),
            Amount = -1m,
            PaidAtUtc = default
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "SupplierId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Method" && x.ErrorMessage.Contains("'Method'"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Amount" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "PaidAtUtc" && x.ErrorMessage.Contains("not be empty"));
    }
}
