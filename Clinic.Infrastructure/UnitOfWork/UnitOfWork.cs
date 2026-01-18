using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Data;
using Clinic.Infrastructure.Repositories;

namespace Clinic.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClinicDbContext _context;

        public UnitOfWork(ClinicDbContext context)
        {
            _context = context;

            Clinics = new GenericRepository<Domain.Entities.Clinic>(_context);
            Doctors = new GenericRepository<Doctor>(_context);
            DoctorSchedules = new GenericRepository<DoctorSchedule>(_context);
            Patients = new GenericRepository<Patient>(_context);
            Appointments = new GenericRepository<Appointment>(_context);
        }

        public IGenericRepository<Domain.Entities.Clinic> Clinics { get; private set; }
        public IGenericRepository<Doctor> Doctors { get; private set; }
        public IGenericRepository<DoctorSchedule> DoctorSchedules { get; private set; }
        public IGenericRepository<Patient> Patients { get; private set; }
        public IGenericRepository<Appointment> Appointments { get; private set; }

        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}

