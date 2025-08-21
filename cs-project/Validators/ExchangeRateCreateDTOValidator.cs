using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class ExchangeRateCreateDTOValidator : AbstractValidator<ExchangeRateCreateDTO>
{
    public ExchangeRateCreateDTOValidator()
    {
        RuleFor(x => x.BaseCurrency)
            .NotEmpty().Length(3);
        RuleFor(x => x.QuoteCurrency)
            .NotEmpty().Length(3);
        RuleFor(x => x.Rate)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.RetrievedAtUtc)
            .NotEmpty();
        RuleFor(x => x.Source)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Source));
    }
}
