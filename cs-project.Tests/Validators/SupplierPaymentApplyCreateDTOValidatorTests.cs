using cs_project.Core.DTOs;
using Xunit;

public class SupplierPaymentApplyCreateDTOValidatorTests
{
    private readonly SupplierPaymentApplyCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new SupplierPaymentApplyCreateDTO
        {
            SupplierPaymentId = 1,
            SupplierInvoiceId = 1,
            AppliedAmount = 50m
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new SupplierPaymentApplyCreateDTO
        {
            SupplierPaymentId = 0,
            SupplierInvoiceId = 0,
            AppliedAmount = -1m
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "SupplierPaymentId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "SupplierInvoiceId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "AppliedAmount" && x.ErrorMessage.Contains("greater than or equal"));
    }
}
