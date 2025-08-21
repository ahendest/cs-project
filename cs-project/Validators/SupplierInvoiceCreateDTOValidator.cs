using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class SupplierInvoiceCreateDTOValidator : AbstractValidator<SupplierInvoiceCreateDTO>
{
    public SupplierInvoiceCreateDTOValidator()
    {
        RuleFor(x => x.SupplierId)
            .GreaterThan(0);
        RuleFor(x => x.StationId)
            .GreaterThan(0);
        RuleFor(x => x.DeliveryDateUtc)
            .NotEmpty();
        RuleFor(x => x.Currency)
            .NotEmpty().Length(3);
        RuleFor(x => x.FxToRon)
            .GreaterThanOrEqualTo(0);
    }
}
