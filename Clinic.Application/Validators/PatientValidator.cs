using Clinic.Domain.Entities;
using FluentValidation;
using System;

namespace Clinic.Application.Validators
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Patient name is required.")
                .MaximumLength(100).WithMessage("Patient name cannot exceed 100 characters.");

            RuleFor(p => p.BirthDate)
                .LessThan(DateTime.Now).WithMessage("Birth date must be in the past.");
        }
    }
}