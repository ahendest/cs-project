using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class CustomerTransactionCreateDTOValidator : AbstractValidator<CustomerTransactionCreateDTO>
{
    public CustomerTransactionCreateDTOValidator()
    {
        RuleFor(x => x.PumpId)
            .GreaterThan(0);
        RuleFor(x => x.StationFuelPriceId)
            .GreaterThan(0);
        RuleFor(x => x.Liters)
            .GreaterThan(0);
        RuleFor(x => x.PricePerLiter)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.TotalPrice)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.TimestampUtc)
            .NotEmpty();
    }
}
