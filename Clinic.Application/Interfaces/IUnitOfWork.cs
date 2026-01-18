using Clinic.Domain.Entities;
namespace Clinic.Application.Interfaces
{
    namespace Clinic.Application.Interfaces
    {
        public interface IUnitOfWork
        {
            IGenericRepository<Domain.Entities.Clinic> Clinics { get; }
            IGenericRepository<Doctor> Doctors { get; }
            IGenericRepository<DoctorSchedule> DoctorSchedules { get; }
            IGenericRepository<Patient> Patients { get; }
            IGenericRepository<Appointment> Appointments { get; }

            Task<int> SaveAsync();
        }
    }

}
