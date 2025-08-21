using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class SupplierPaymentCreateDTOValidator : AbstractValidator<SupplierPaymentCreateDTO>
{
    public SupplierPaymentCreateDTOValidator()
    {
        RuleFor(x => x.SupplierId)
            .GreaterThan(0);
        RuleFor(x => x.Method)
            .IsInEnum();
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.PaidAtUtc)
            .NotEmpty();
    }
}
