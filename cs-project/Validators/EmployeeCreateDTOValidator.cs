using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class EmployeeCreateDTOValidator : AbstractValidator<EmployeeCreateDTO>
{
    public EmployeeCreateDTOValidator()
    {
        RuleFor(x => x.StationId)
            .GreaterThan(0).When(x => x.StationId.HasValue);
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100);
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100);
        RuleFor(x => x.Role)
            .IsInEnum();
        RuleFor(x => x.HireDateUtc)
            .NotEmpty();
    }
}
