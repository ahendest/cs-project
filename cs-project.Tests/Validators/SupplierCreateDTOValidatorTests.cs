using cs_project.Core.DTOs;
using Xunit;

public class SupplierCreateDTOValidatorTests
{
    private readonly SupplierCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new SupplierCreateDTO
        {
            CompanyName = "Company",
            ContactPerson = "Person",
            Phone = "+1234567890",
            Email = "test@example.com",
            TaxRegistrationNumber = "12345"
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new SupplierCreateDTO
        {
            CompanyName = "",
            ContactPerson = new string('a', 101),
            Phone = "invalid",
            Email = "invalid",
            TaxRegistrationNumber = new string('a', 51)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "CompanyName" && x.ErrorMessage.Contains("required"));
        Assert.Contains(result.Errors, x => x.PropertyName == "ContactPerson" && x.ErrorMessage.Contains("characters"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Phone" && x.ErrorMessage.Contains("not valid"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Email" && x.ErrorMessage.Contains("email address"));
        Assert.Contains(result.Errors, x => x.PropertyName == "TaxRegistrationNumber" && x.ErrorMessage.Contains("characters"));
    }
}
