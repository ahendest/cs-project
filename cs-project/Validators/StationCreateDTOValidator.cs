using cs_project.Core.DTOs;
using FluentValidation;

public class StationCreateDTOValidator : AbstractValidator<StationCreateDTO>
{
    public StationCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(200);
        RuleFor(x => x.City)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.City));
        RuleFor(x => x.Country)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Country));
    }
}
