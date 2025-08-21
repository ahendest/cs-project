using System;
using cs_project.Core.DTOs;
using FluentValidation;

public class TankCreateDTOValidator : AbstractValidator<TankCreateDTO>
{
    public TankCreateDTOValidator()
    {
        RuleFor(x => x.StationId)
            .GreaterThan(0);
        RuleFor(x => x.FuelType)
            .IsInEnum();
        RuleFor(x => x.CapacityLiters)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.CurrentVolumeLiters)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.CapacityLiters);
        RuleFor(x => x.LastRefilledAtUtc)
            .LessThanOrEqualTo(DateTime.UtcNow).When(x => x.LastRefilledAtUtc.HasValue);
    }
}
