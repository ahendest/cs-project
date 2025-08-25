using System;
using cs_project.Core.DTOs;
using Xunit;

public class ShiftCreateDTOValidatorTests
{
    private readonly ShiftCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new ShiftCreateDTO
        {
            StationId = 1,
            StartUtc = DateTime.UtcNow,
            EndUtc = DateTime.UtcNow.AddHours(8),
            TotalSalesAmount = 0m
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new ShiftCreateDTO
        {
            StationId = 0,
            StartUtc = default,
            EndUtc = default,
            TotalSalesAmount = -1m
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "StationId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "StartUtc" && x.ErrorMessage.Contains("not be empty"));
        Assert.Contains(result.Errors, x => x.PropertyName == "EndUtc" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "TotalSalesAmount" && x.ErrorMessage.Contains("greater than or equal"));
    }
}
