using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class StationFuelPriceCreateDTOValidator : AbstractValidator<StationFuelPriceCreateDTO>
{
    public StationFuelPriceCreateDTOValidator()
    {
        RuleFor(x => x.StationId)
            .GreaterThan(0);
        RuleFor(x => x.FuelType)
            .IsInEnum();
        RuleFor(x => x.PriceRon)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.EffectiveFromUtc)
            .NotEmpty();
        RuleFor(x => x.EffectiveToUtc)
            .GreaterThan(x => x.EffectiveFromUtc).When(x => x.EffectiveToUtc.HasValue);
        RuleFor(x => x.DerivedFromPolicyId)
            .GreaterThan(0).When(x => x.DerivedFromPolicyId.HasValue);
        RuleFor(x => x.FxRateUsed)
            .GreaterThanOrEqualTo(0).When(x => x.FxRateUsed.HasValue);
        RuleFor(x => x.CostRonUsed)
            .GreaterThanOrEqualTo(0).When(x => x.CostRonUsed.HasValue);
        RuleFor(x => x.CreatedByEmployeeId)
            .GreaterThan(0).When(x => x.CreatedByEmployeeId.HasValue);
    }
}
