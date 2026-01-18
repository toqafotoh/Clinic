using Clinic.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicEntity = Clinic.Domain.Entities.Clinic;
namespace Clinic.Application.Interfaces.Services
{
    public interface IClinicService
    {
        Task<IEnumerable<ClinicEntity>> GetAllAsync();
        Task<ClinicEntity?> GetByIdAsync(int id);
        Task AddAsync(ClinicEntity clinic);
        Task UpdateAsync(ClinicEntity clinic);
        Task DeleteAsync(ClinicEntity clinic);
    }
}
