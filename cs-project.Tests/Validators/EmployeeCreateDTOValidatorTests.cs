using System;
using cs_project.Core.DTOs;
using static cs_project.Core.Entities.Enums;
using Xunit;

public class EmployeeCreateDTOValidatorTests
{
    private readonly EmployeeCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new EmployeeCreateDTO
        {
            StationId = 1,
            FirstName = "John",
            LastName = "Doe",
            Role = EmployeeRole.Cashier,
            HireDateUtc = DateTime.UtcNow
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new EmployeeCreateDTO
        {
            StationId = 0,
            FirstName = "",
            LastName = "",
            Role = (EmployeeRole)(-1),
            HireDateUtc = default
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "StationId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "FirstName" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "LastName" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Role" && x.ErrorMessage.Contains("'Role'"));
        Assert.Contains(result.Errors, x => x.PropertyName == "HireDateUtc" && x.ErrorMessage.Contains("not be empty"));
    }
}
