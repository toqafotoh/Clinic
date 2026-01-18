using Clinic.Domain.Entities;
using FluentValidation;
using System;

namespace Clinic.Application.Validators
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(a => a.DoctorId)
                .GreaterThan(0).WithMessage("You must select a doctor.");

            RuleFor(a => a.PatientId)
                .GreaterThan(0).When(a => a.PatientId != null)
                .WithMessage("You must select a patient.");

            RuleFor(a => a.AppointmentDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Appointment date cannot be in the past.");

            RuleFor(a => a.VisitDuration)
                .GreaterThan(0).WithMessage("Visit duration must be positive.")
                .LessThanOrEqualTo(240).WithMessage("Visit duration cannot exceed 4 hours.");
        }
    }
}