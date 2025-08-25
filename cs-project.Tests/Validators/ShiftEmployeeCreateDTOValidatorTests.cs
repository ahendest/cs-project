using cs_project.Core.DTOs;
using Xunit;

public class ShiftEmployeeCreateDTOValidatorTests
{
    private readonly ShiftEmployeeCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new ShiftEmployeeCreateDTO
        {
            ShiftId = 1,
            EmployeeId = 1
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new ShiftEmployeeCreateDTO
        {
            ShiftId = 0,
            EmployeeId = 0
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "ShiftId" && x.ErrorMessage.Contains("greater than"));
        Assert.Contains(result.Errors, x => x.PropertyName == "EmployeeId" && x.ErrorMessage.Contains("greater than"));
    }
}
