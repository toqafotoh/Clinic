using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clinic.Domain.Entities;
using System.Reflection.Emit;

namespace Clinic.Infrastructure.Data
{
    public class ClinicDbContext : DbContext
    {
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options) { }

        public DbSet<Domain.Entities.Clinic> Clinics { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Clinic)
                .WithMany(c => c.Doctors)
                .HasForeignKey(d => d.ClinicId);

            modelBuilder.Entity<DoctorSchedule>()
                .HasOne(s => s.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);
            
            //modelBuilder.Entity<Domain.Entities.Clinic>().HasKey(c => c.ClinicId);
            //modelBuilder.Entity<Doctor>().HasKey(d => d.DoctorId);
            //modelBuilder.Entity<Patient>().HasKey(p => p.PatientId);
            //modelBuilder.Entity<Appointment>().HasKey(a => a.AppointmentId);
            //modelBuilder.Entity<DoctorSchedule>()
            //    .HasKey(s => s.DoctorScheduleId);

        }
    }
}
