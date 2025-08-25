using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class TankCreateDTOValidatorTests
{
    private readonly TankCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new TankCreateDTO
        {
            StationId = 1,
            FuelType = FuelType.Petrol95,
            CapacityLiters = 1000m,
            CurrentVolumeLiters = 500m,
            LastRefilledAtUtc = DateTime.UtcNow.AddDays(-1)
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new TankCreateDTO
        {
            StationId = 0,
            FuelType = (FuelType)(-1),
            CapacityLiters = -1m,
            CurrentVolumeLiters = 2000m,
            LastRefilledAtUtc = DateTime.UtcNow.AddDays(1)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "StationId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FuelType" && x.ErrorMessage.Contains("range"));
        Assert.Contains(result.Errors, x => x.PropertyName == "CapacityLiters" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "CurrentVolumeLiters" && x.ErrorMessage.Contains("less than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "LastRefilledAtUtc" && x.ErrorMessage.Contains("less than or equal"));
    }
}
