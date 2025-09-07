using System;
using cs_project.Core.DTOs;
using Xunit;

public class AuditLogCreateDTOValidatorTests
{
    private readonly AuditLogCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new AuditLogCreateDTO
        {
            TableName = "Users",
            RecordId = 1,
            Operation = "Insert",
            ModifiedBy = Guid.NewGuid().ToString(),
            ModifiedAt = DateTimeOffset.UtcNow
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new AuditLogCreateDTO
        {
            TableName = "",
            RecordId = 0,
            Operation = "",
            ModifiedBy = "not-a-guid",
            ModifiedAt = default
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "TableName" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "RecordId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Operation" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "ModifiedBy" && x.ErrorMessage.Contains("valid GUID"));
        Assert.Contains(result.Errors, x => x.PropertyName == "ModifiedAt" && x.ErrorMessage.Contains("not be empty"));
    }
}
