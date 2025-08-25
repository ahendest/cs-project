using System;
using cs_project.Core.DTOs;
using Xunit;

public class CustomerTransactionCreateDTOValidatorTests
{
    private readonly CustomerTransactionCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CustomerTransactionCreateDTO
        {
            PumpId = 1,
            StationFuelPriceId = 1,
            Liters = 10m,
            PricePerLiter = 5m,
            TotalPrice = 50m,
            TimestampUtc = DateTimeOffset.UtcNow
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new CustomerTransactionCreateDTO
        {
            PumpId = 0,
            StationFuelPriceId = 0,
            Liters = 0m,
            PricePerLiter = -1m,
            TotalPrice = -1m,
            TimestampUtc = default
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "PumpId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "StationFuelPriceId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Liters" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "PricePerLiter" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "TotalPrice" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "TimestampUtc" && x.ErrorMessage.Contains("not be empty"));
    }
}
