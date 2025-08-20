using System;
using cs_project.Core.DTOs;
using Xunit;

public class PumpCreateDTOValidatorTests
{
    private readonly PumpCreateDTOValidator _validator = new PumpCreateDTOValidator();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new PumpCreateDTO
        {
            FuelType = "Diesel",
            CurrentVolume = 150.0,
            Status = "Idle"
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_FuelType_Is_Empty()
    {
        var dto = new PumpCreateDTO
        {
            FuelType = "",
            CurrentVolume = 100,
            Status = "Idle"
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "FuelType");
    }

    [Fact]
    public void Should_Fail_When_FuelType_Exceeds_Max_Length()
    {
        var dto = new PumpCreateDTO
        {
            FuelType = new string('A', 51), // 51 chars
            CurrentVolume = 100,
            Status = "Idle"
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "FuelType");
    }

    [Fact]
    public void Should_Fail_When_CurrentVolume_Is_Negative()
    {
        var dto = new PumpCreateDTO
        {
            FuelType = "Gasoline",
            CurrentVolume = -1, // Invalid
            Status = "Idle"
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "CurrentVolume");
    }

    [Fact]
    public void Should_Fail_When_Status_Is_Empty()
    {
        var dto = new PumpCreateDTO
        {
            FuelType = "Diesel",
            CurrentVolume = 100,
            Status = ""
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Status");
    }
}
