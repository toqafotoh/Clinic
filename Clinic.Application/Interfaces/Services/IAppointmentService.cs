using Clinic.Application.Common;
using Clinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);

        Task<OperationResult> CreateAppointmentSlotAsync(int doctorId, DateTime date, TimeSpan startTime);

        Task<OperationResult> BookAppointmentAsync(int appointmentId, int patientId);

        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);

        Task<List<TimeSpan>> GetFreeSlotsAsync(int doctorId, DateTime date);
        Task<List<Appointment>> GetAvailableAppointmentsAsync(int doctorId, DateTime date);
    }
}
