using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class StationFuelPriceCreateDTOValidatorTests
{
    private readonly StationFuelPriceCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new StationFuelPriceCreateDTO
        {
            StationId = 1,
            FuelType = FuelType.Diesel,
            PriceRon = 5m,
            EffectiveFromUtc = DateTime.UtcNow,
            EffectiveToUtc = DateTime.UtcNow.AddDays(1),
            DerivedFromPolicyId = 1,
            FxRateUsed = 4.5m,
            CostRonUsed = 3m,
            Reason = "Reason",
            CreatedByEmployeeId = 1
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new StationFuelPriceCreateDTO
        {
            StationId = 0,
            FuelType = (FuelType)(-1),
            PriceRon = -1m,
            EffectiveFromUtc = DateTime.UtcNow,
            EffectiveToUtc = DateTime.UtcNow.AddMinutes(-1),
            DerivedFromPolicyId = 0,
            FxRateUsed = -1m,
            CostRonUsed = -1m,
            CreatedByEmployeeId = 0
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "StationId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FuelType" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "PriceRon" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "EffectiveToUtc" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "DerivedFromPolicyId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FxRateUsed" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "CostRonUsed" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "CreatedByEmployeeId" && x.ErrorMessage.Contains("greater than"));
    }
}
