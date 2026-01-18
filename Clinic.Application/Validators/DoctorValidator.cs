using Clinic.Domain.Entities;
using FluentValidation;

namespace Clinic.Application.Validators
{
    public class DoctorValidator : AbstractValidator<Doctor>
    {
        public DoctorValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Doctor name is required.")
                .MaximumLength(100).WithMessage("Doctor name cannot exceed 100 characters.");

            RuleFor(d => d.ClinicId)
                .GreaterThan(0).WithMessage("Doctor must belong to a clinic.");
        }
    }
}