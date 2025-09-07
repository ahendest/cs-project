using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class AuditLogCreateDTOValidator : AbstractValidator<AuditLogCreateDTO>
{
    public AuditLogCreateDTOValidator()
    {
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("Table name is required.")
            .MaximumLength(100);
        RuleFor(x => x.RecordId)
            .GreaterThan(0);
        RuleFor(x => x.Operation)
            .NotEmpty().WithMessage("Operation is required.")
            .MaximumLength(50);
        RuleFor(x => x.ModifiedBy)
            .NotEmpty()
            .Must(id => Guid.TryParse(id, out _))
            .WithMessage("ModifiedBy must be a valid GUID.");
        RuleFor(x => x.ModifiedAt)
            .NotEmpty();
    }
}
