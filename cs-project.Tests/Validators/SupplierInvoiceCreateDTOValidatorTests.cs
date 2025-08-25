using System;
using cs_project.Core.DTOs;
using Xunit;

public class SupplierInvoiceCreateDTOValidatorTests
{
    private readonly SupplierInvoiceCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new SupplierInvoiceCreateDTO
        {
            SupplierId = 1,
            StationId = 1,
            DeliveryDateUtc = DateTime.UtcNow,
            Currency = "USD",
            FxToRon = 4.5m
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new SupplierInvoiceCreateDTO
        {
            SupplierId = 0,
            StationId = 0,
            DeliveryDateUtc = default,
            Currency = "US",
            FxToRon = -1m
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "SupplierId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "StationId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "DeliveryDateUtc" && x.ErrorMessage.Contains("not be empty"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Currency" && x.ErrorMessage.Contains("length"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FxToRon" && x.ErrorMessage.Contains("greater than or equal"));
    }
}
