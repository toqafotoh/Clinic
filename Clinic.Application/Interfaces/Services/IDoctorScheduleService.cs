using Clinic.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Application.Interfaces.Services
{
    public interface IDoctorScheduleService
    {
        Task<IEnumerable<DoctorSchedule>> GetAllAsync();
        Task<DoctorSchedule?> GetByIdAsync(int id);
        Task AddAsync(DoctorSchedule schedule);
        Task UpdateAsync(DoctorSchedule schedule);
        Task DeleteAsync(DoctorSchedule schedule);
    }
}
