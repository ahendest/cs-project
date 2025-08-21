using cs_project.Core.DTOs;
using FluentValidation;

public class ShiftEmployeeCreateDTOValidator : AbstractValidator<ShiftEmployeeCreateDTO>
{
    public ShiftEmployeeCreateDTOValidator()
    {
        RuleFor(x => x.ShiftId)
            .GreaterThan(0);
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0);
    }
}
