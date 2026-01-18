using Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync() => await _unitOfWork.Patients.GetAllAsync();
        public async Task<Patient?> GetByIdAsync(int id) => await _unitOfWork.Patients.GetByIdAsync(id);

        public async Task AddAsync(Patient patient)
        {
            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Patient patient)
        {
            _unitOfWork.Patients.Update(patient);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(Patient patient)
        {
            _unitOfWork.Patients.Delete(patient);
            await _unitOfWork.SaveAsync();
        }
    }
}
