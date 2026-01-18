using Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicEntity = Clinic.Domain.Entities.Clinic;
namespace Clinic.Application.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ClinicEntity>> GetAllAsync() =>
            await _unitOfWork.Clinics.GetAllAsync();

        public async Task<ClinicEntity?> GetByIdAsync(int id) =>
            await _unitOfWork.Clinics.GetByIdAsync(id);

        public async Task AddAsync(ClinicEntity clinic)
        {
            await _unitOfWork.Clinics.AddAsync(clinic);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ClinicEntity clinic)
        {
            _unitOfWork.Clinics.Update(clinic);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(ClinicEntity clinic)
        {
            _unitOfWork.Clinics.Delete(clinic);
            await _unitOfWork.SaveAsync();
        }
    }
}
