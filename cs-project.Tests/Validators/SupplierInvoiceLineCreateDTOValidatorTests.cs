using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class SupplierInvoiceLineCreateDTOValidatorTests
{
    private readonly SupplierInvoiceLineCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new SupplierInvoiceLineCreateDTO
        {
            SupplierInvoiceId = 1,
            TankId = 1,
            FuelType = FuelType.Diesel,
            QuantityLiters = 100m,
            UnitPrice = 2m
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new SupplierInvoiceLineCreateDTO
        {
            SupplierInvoiceId = 0,
            TankId = 0,
            FuelType = (FuelType)(-1),
            QuantityLiters = 0m,
            UnitPrice = -1m
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "SupplierInvoiceId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "TankId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FuelType" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "QuantityLiters" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "UnitPrice" && x.ErrorMessage.Contains("greater than or equal"));
    }
}
