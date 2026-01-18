using Clinic.Application.Common;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;

namespace Clinic.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _unitOfWork.Appointments.GetAllAsync(
                a => a.Doctor,
                a => a.Patient
            );
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Appointments.GetByIdAsync(
                id,
                a => a.Doctor,
                a => a.Patient
            );
        }

        public async Task<OperationResult> CreateAppointmentSlotAsync(int doctorId, DateTime date, TimeSpan startTime)
        {
            bool exists = await _unitOfWork.Appointments.Exists(a =>
                a.DoctorId == doctorId &&
                a.AppointmentDate.Date == date.Date &&
                a.StartTime == startTime);

            if (exists)
                return OperationResult.Fail("This slot already exists.");

            var schedules = (await _unitOfWork.DoctorSchedules.GetAllAsync())
                .Where(s => s.DoctorId == doctorId && s.DayOfWeek == date.DayOfWeek);

            bool fitsSchedule = schedules.Any(s => startTime >= s.StartTime && startTime + TimeSpan.FromMinutes(30) <= s.EndTime);

            if (!fitsSchedule)
                return OperationResult.Fail("Slot is outside doctor's schedule.");

            var duration = TimeSpan.FromMinutes(30); 

            bool overlaps = (await _unitOfWork.Appointments.GetAllAsync())
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date)
                .Any(a =>
                    a.StartTime < startTime + duration &&
                    startTime < a.StartTime + duration);

            if (overlaps)
                return OperationResult.Fail("This slot overlaps with an existing appointment.");

            var slot = new Appointment
            {
                DoctorId = doctorId,
                AppointmentDate = date.Date,
                StartTime = startTime,
                PatientId = null
            };

            await _unitOfWork.Appointments.AddAsync(slot);
            await _unitOfWork.SaveAsync();

            return OperationResult.Ok("Slot created successfully!");
        }

        public async Task<OperationResult> BookAppointmentAsync(int appointmentId, int patientId)
        {
            var slot = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);
            if (slot == null)
                return OperationResult.Fail("Selected slot does not exist.");

            if (slot.PatientId != null)
                return OperationResult.Fail("This slot is already booked.");

            slot.PatientId = patientId;
            _unitOfWork.Appointments.Update(slot);
            await _unitOfWork.SaveAsync();

            return OperationResult.Ok("Appointment booked successfully!");
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _unitOfWork.Appointments.Update(appointment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _unitOfWork.Appointments.Delete(appointment);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<TimeSpan>> GetFreeSlotsAsync(int doctorId, DateTime date)
        {
            var schedules = (await _unitOfWork.DoctorSchedules.GetAllAsync())
                .Where(s => s.DoctorId == doctorId && s.DayOfWeek == date.DayOfWeek)
                .ToList();

            var booked = (await _unitOfWork.Appointments.GetAllAsync())
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate.Date == date.Date && a.PatientId != null)
                .Select(a => a.StartTime)
                .ToList();

            var freeSlots = new List<TimeSpan>();

            foreach (var schedule in schedules)
            {
                var time = schedule.StartTime;
                while (time.Add(TimeSpan.FromMinutes(30)) <= schedule.EndTime)
                {
                    if (!booked.Any(b => time < b.Add(TimeSpan.FromMinutes(30)) && time.Add(TimeSpan.FromMinutes(30)) > b))
                        freeSlots.Add(time);

                    time = time.Add(TimeSpan.FromMinutes(30));
                }
            }

            return freeSlots;
        }
        public async Task<List<Appointment>> GetAvailableAppointmentsAsync(int doctorId, DateTime date)
        {
            var appointments = (await _unitOfWork.Appointments.GetAllAsync())
                .Where(a => a.DoctorId == doctorId &&
                           a.AppointmentDate.Date == date.Date &&
                           a.PatientId == null)
                .OrderBy(a => a.StartTime)
                .ToList();

            return appointments;
        }
    }
}