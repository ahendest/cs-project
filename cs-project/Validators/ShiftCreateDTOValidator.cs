using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class ShiftCreateDTOValidator : AbstractValidator<ShiftCreateDTO>
{
    public ShiftCreateDTOValidator()
    {
        RuleFor(x => x.StationId)
            .GreaterThan(0);
        RuleFor(x => x.StartUtc)
            .NotEmpty();
        RuleFor(x => x.EndUtc)
            .GreaterThan(x => x.StartUtc);
        RuleFor(x => x.TotalSalesAmount)
            .GreaterThanOrEqualTo(0);
    }
}
