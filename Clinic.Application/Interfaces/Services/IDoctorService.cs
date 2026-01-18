using Clinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clinic.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor?> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        Task UpdateAsync(Doctor doctor);
        Task DeleteAsync(Doctor doctor);
    }
}