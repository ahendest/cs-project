using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class CustomerPaymentCreateDTOValidator : AbstractValidator<CustomerPaymentCreateDTO>
{
    public CustomerPaymentCreateDTOValidator()
    {
        RuleFor(x => x.CustomerTransactionId)
            .GreaterThan(0);
        RuleFor(x => x.Method)
            .IsInEnum();
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.AuthorizationCode)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.AuthorizationCode));
        RuleFor(x => x.ProcessedAtUtc)
            .NotEmpty();
    }
}
