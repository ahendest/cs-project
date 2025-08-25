using cs_project.Core.DTOs;
using Xunit;

public class StationCreateDTOValidatorTests
{
    private readonly StationCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new StationCreateDTO
        {
            Name = "Station",
            Address = "Main St",
            City = "City",
            Country = "Country"
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new StationCreateDTO
        {
            Name = "",
            Address = "",
            City = new string('a', 101),
            Country = new string('a', 101)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "Name" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Address" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "City" && x.ErrorMessage.Contains("characters"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Country" && x.ErrorMessage.Contains("characters"));
    }
}
