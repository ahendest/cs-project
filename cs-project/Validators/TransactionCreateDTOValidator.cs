using cs_project.Core.DTOs;
using FluentValidation;

namespace cs_project.Validators
{
    public class TransactionCreateDTOValidator : AbstractValidator<TransactionsCreateDTO>
    {
        public TransactionCreateDTOValidator()
        {
            RuleFor(x => x.PumpId)
                .GreaterThan(0).WithMessage("PumpId must be a positive integer.");

            RuleFor(x => x.Liters)
                .GreaterThan(0).WithMessage("Liters must be greater than 0.");

            RuleFor(x => x.PricePerLiter)
                .GreaterThan(0).WithMessage("PricePerLiter must be greater than 0.");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("TotalPrice must be 0 or greater.");

            RuleFor(x => x.Timestamp)
                .NotEmpty().WithMessage("Timestamp is required.")
                .LessThanOrEqualTo(_ => DateTime.UtcNow).WithMessage("Timestamp cannot be in the future.");
        }
    }
}
