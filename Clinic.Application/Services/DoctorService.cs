using Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            // Include Clinic navigation property
            return await _unitOfWork.Doctors.GetAllAsync(d => d.Clinic);
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            // Include all related entities for details page
            return await _unitOfWork.Doctors.GetByIdAsync(
                id,
                d => d.Clinic,
                d => d.Schedules,
                d => d.Appointments
            );
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _unitOfWork.Doctors.AddAsync(doctor);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _unitOfWork.Doctors.Update(doctor);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Doctor doctor)
        {
            _unitOfWork.Doctors.Delete(doctor);
            await _unitOfWork.SaveAsync();
        }
    }
}