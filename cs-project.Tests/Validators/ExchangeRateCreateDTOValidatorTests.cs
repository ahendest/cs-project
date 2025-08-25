using System;
using cs_project.Core.DTOs;
using Xunit;

public class ExchangeRateCreateDTOValidatorTests
{
    private readonly ExchangeRateCreateDTOValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new ExchangeRateCreateDTO
        {
            BaseCurrency = "USD",
            QuoteCurrency = "RON",
            Rate = 4.5m,
            RetrievedAtUtc = DateTime.UtcNow,
            Source = "ECB"
        };

        var result = _validator.Validate(dto);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Should_Fail_When_Invalid()
    {
        var dto = new ExchangeRateCreateDTO
        {
            BaseCurrency = "US",
            QuoteCurrency = "",
            Rate = -1m,
            RetrievedAtUtc = default,
            Source = new string('a', 101)
        };

        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, x => x.PropertyName == "BaseCurrency" && x.ErrorMessage.Contains("length"));
        Assert.Contains(result.Errors, x => x.PropertyName == "QuoteCurrency" && x.ErrorMessage.Contains("length"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Rate" && x.ErrorMessage.Contains("greater than or equal"));
        Assert.Contains(result.Errors, x => x.PropertyName == "RetrievedAtUtc" && x.ErrorMessage.Contains("not be empty"));
        Assert.Contains(result.Errors, x => x.PropertyName == "Source" && x.ErrorMessage.Contains("characters"));
    }
}
