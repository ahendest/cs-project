using cs_project.Core.DTOs;
using FluentValidation;

public class SupplierPaymentApplyCreateDTOValidator : AbstractValidator<SupplierPaymentApplyCreateDTO>
{
    public SupplierPaymentApplyCreateDTOValidator()
    {
        RuleFor(x => x.SupplierPaymentId)
            .GreaterThan(0);
        RuleFor(x => x.SupplierInvoiceId)
            .GreaterThan(0);
        RuleFor(x => x.AppliedAmount)
            .GreaterThanOrEqualTo(0);
    }
}
