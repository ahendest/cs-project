using cs_project.Core.DTOs;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

public class SupplierCreateDTOValidator : AbstractValidator<SupplierCreateDTO>
{
    public SupplierCreateDTOValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200);
        RuleFor(x => x.ContactPerson)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.ContactPerson));
        RuleFor(x => x.Phone)
            .Must(phone => new PhoneAttribute().IsValid(phone!))
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone is not valid.");
        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.TaxRegistrationNumber)
            .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.TaxRegistrationNumber));
    }
}
