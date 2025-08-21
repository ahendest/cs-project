using cs_project.Core.DTOs;
using FluentValidation;

public class SupplierInvoiceLineCreateDTOValidator : AbstractValidator<SupplierInvoiceLineCreateDTO>
{
    public SupplierInvoiceLineCreateDTOValidator()
    {
        RuleFor(x => x.SupplierInvoiceId)
            .GreaterThan(0);
        RuleFor(x => x.TankId)
            .GreaterThan(0);
        RuleFor(x => x.FuelType)
            .IsInEnum();
        RuleFor(x => x.QuantityLiters)
            .GreaterThan(0);
        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0);
    }
}
