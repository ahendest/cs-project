using cs_project.Core.DTOs;
using FluentValidation;

public class PumpCreateDTOValidator : AbstractValidator<PumpCreateDTO>
{
	public PumpCreateDTOValidator()
	{
		RuleFor(x => x.FuelType)
			.NotEmpty().WithMessage("Fuel type is required.")
			.MaximumLength(50);
		RuleFor(x => x.CurrentVolume)
			.GreaterThanOrEqualTo(0).WithMessage("Current price should be at least 0 Euro.");
		RuleFor(x => x.Status)
			.NotEmpty().WithMessage("Status can not be empty.");
	}
}
