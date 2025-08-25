using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class CorrectionLogCreateDTOValidatorTests
{
    private readonly CorrectionLogCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CorrectionLogCreateDTO
        {
            TargetTable = "Logs",
            TargetId = 1,
            Type = CorrectionType.Adjustment,
            Reason = "Reason",
            RequestedById = 1,
            ApprovedById = 2,
            RequestedAtUtc = DateTime.UtcNow,
            ApprovedAtUtc = DateTime.UtcNow.AddHours(1)
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var now = DateTime.UtcNow;
        var dto = new CorrectionLogCreateDTO
        {
            TargetTable = "",
            TargetId = 0,
            Type = (CorrectionType)(-1),
            Reason = "",
            RequestedById = 0,
            ApprovedById = 0,
            RequestedAtUtc = now,
            ApprovedAtUtc = now
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "TargetTable" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "TargetId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Type" && x.ErrorMessage.Contains("'Type'"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Reason" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "RequestedById" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "ApprovedById" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "ApprovedAtUtc" && x.ErrorMessage.Contains("greater than"));
    }
}
