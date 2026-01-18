using Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Services;
using Clinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Application.Services
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DoctorScheduleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DoctorSchedule>> GetAllAsync()
        {
            return await _unitOfWork.DoctorSchedules.GetAllAsync(s => s.Doctor);
        }

        public async Task<DoctorSchedule?> GetByIdAsync(int id)
        {
            var schedules = await _unitOfWork.DoctorSchedules.GetAllAsync(s => s.Doctor);
            return schedules.FirstOrDefault(s => s.DoctorScheduleId == id);
        }
        public async Task AddAsync(DoctorSchedule schedule)
        {
            await _unitOfWork.DoctorSchedules.AddAsync(schedule);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(DoctorSchedule schedule)
        {
            _unitOfWork.DoctorSchedules.Update(schedule);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(DoctorSchedule schedule)
        {
            _unitOfWork.DoctorSchedules.Delete(schedule);
            await _unitOfWork.SaveAsync();
        }
    }
}
