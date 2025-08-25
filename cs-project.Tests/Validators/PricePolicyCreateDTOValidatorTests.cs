using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class PricePolicyCreateDTOValidatorTests
{
    private readonly PricePolicyCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new PricePolicyCreateDTO
        {
            StationId = 1,
            FuelType = FuelType.Petrol95,
            Method = PricingMethod.FlatUsd,
            BaseUsdPrice = 1m,
            MarginPct = 10m,
            MarginRon = 0m,
            RoundingIncrement = 0.1m,
            RoundingMode = RoundingMode.Round,
            EffectiveFromUtc = DateTime.UtcNow,
            EffectiveToUtc = DateTime.UtcNow.AddDays(1),
            Priority = 1
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new PricePolicyCreateDTO
        {
            StationId = 0,
            FuelType = (FuelType)(-1),
            Method = (PricingMethod)(-1),
            BaseUsdPrice = -1m,
            MarginPct = -1m,
            MarginRon = -1m,
            RoundingIncrement = -1m,
            RoundingMode = (RoundingMode)(-1),
            EffectiveFromUtc = default,
            EffectiveToUtc = DateTime.MinValue,
            Priority = -1
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "StationId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FuelType" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Method" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "BaseUsdPrice" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "MarginPct" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "MarginRon" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "RoundingIncrement" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "RoundingMode" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "EffectiveFromUtc" && x.ErrorMessage.Contains("not be empty"));
        Assert.Contains(result.Errors, x => x.PropertyName == "EffectiveToUtc" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Priority" && x.ErrorMessage.Contains("greater than or equal"));
    }
}
