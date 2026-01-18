using Clinic.Domain.Entities;
using FluentValidation;

namespace Clinic.Application.Validators
{
    public class ClinicValidator : AbstractValidator<Domain.Entities.Clinic>
    {
        public ClinicValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Clinic name is required.")
                .MaximumLength(200).WithMessage("Clinic name cannot exceed 200 characters.");
        }
    }
}