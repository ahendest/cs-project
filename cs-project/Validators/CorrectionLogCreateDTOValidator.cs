using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class CorrectionLogCreateDTOValidator : AbstractValidator<CorrectionLogCreateDTO>
{
    public CorrectionLogCreateDTOValidator()
    {
        RuleFor(x => x.TargetTable)
            .NotEmpty().WithMessage("Target table is required.")
            .MaximumLength(100);
        RuleFor(x => x.TargetId)
            .GreaterThan(0);
        RuleFor(x => x.Type)
            .IsInEnum();
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.");
        RuleFor(x => x.RequestedById)
            .GreaterThan(0);
        RuleFor(x => x.ApprovedById)
            .GreaterThan(0).When(x => x.ApprovedById.HasValue);
        RuleFor(x => x.ApprovedAtUtc)
            .GreaterThan(x => x.RequestedAtUtc).When(x => x.ApprovedAtUtc.HasValue);
    }
}
