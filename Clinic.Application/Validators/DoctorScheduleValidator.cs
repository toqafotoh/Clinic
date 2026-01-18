using Clinic.Domain.Entities;
using FluentValidation;

namespace Clinic.Application.Validators
{
    public class DoctorScheduleValidator : AbstractValidator<DoctorSchedule>
    {
        public DoctorScheduleValidator()
        {
            RuleFor(s => s.DoctorId)
                .GreaterThan(0).WithMessage("Doctor must be selected.");

            RuleFor(s => s.DayOfWeek)
                .IsInEnum().WithMessage("Invalid day of week.");

            RuleFor(s => s.StartTime)
                .LessThan(s => s.EndTime).WithMessage("Start time must be before end time.");
        }
    }
}