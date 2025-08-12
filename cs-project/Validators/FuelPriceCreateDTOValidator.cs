using cs_project.Core.DTOs;
using FluentValidation;

namespace cs_project.Validators
{
    public class FuelPriceCreateDTOValidator : AbstractValidator<FuelPriceCreateDTO>
    {
        public FuelPriceCreateDTOValidator()
        {
            RuleFor(x => x.FuelType)
                .NotNull().WithMessage("Fuel type must not be null.")
                .NotEmpty().WithMessage("Fuel type is required.");
            RuleFor(x => x.CurrentPrice)
                .GreaterThan(0).WithMessage("Current price should be at least 0 Euro.");
            RuleFor(x => x.UpdatedAt)
                .NotEmpty().WithMessage("Time stamp can not be empty.")
                .LessThanOrEqualTo(_ => DateTime.UtcNow)
                .WithMessage("Time stamp cannot be in the future.");
        }
    }
}
