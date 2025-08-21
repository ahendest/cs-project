using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class PricePolicyCreateDTOValidator : AbstractValidator<PricePolicyCreateDTO>
{
    public PricePolicyCreateDTOValidator()
    {
        RuleFor(x => x.StationId)
            .GreaterThan(0).When(x => x.StationId.HasValue);
        RuleFor(x => x.FuelType)
            .IsInEnum();
        RuleFor(x => x.Method)
            .IsInEnum();
        RuleFor(x => x.BaseUsdPrice)
            .GreaterThanOrEqualTo(0).When(x => x.BaseUsdPrice.HasValue);
        RuleFor(x => x.MarginPct)
            .GreaterThanOrEqualTo(0).When(x => x.MarginPct.HasValue);
        RuleFor(x => x.MarginRon)
            .GreaterThanOrEqualTo(0).When(x => x.MarginRon.HasValue);
        RuleFor(x => x.RoundingIncrement)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.RoundingMode)
            .IsInEnum();
        RuleFor(x => x.EffectiveFromUtc)
            .NotEmpty();
        RuleFor(x => x.EffectiveToUtc)
            .GreaterThan(x => x.EffectiveFromUtc).When(x => x.EffectiveToUtc.HasValue);
        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0);
    }
}
