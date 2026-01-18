using Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Clinic.Application.Interfaces;
using Clinic.Application.Interfaces.Services;
using Clinic.Application.Services;
using Clinic.Application.Validators;
using Clinic.Domain.Entities;
using Clinic.Infrastructure;
using Clinic.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Clinic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container

            // Database Context
            builder.Services.AddDbContext<ClinicDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Services
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IClinicService, ClinicService>();
            builder.Services.AddScoped<IDoctorScheduleService, DoctorScheduleService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IPatientService, PatientService>();

            // FluentValidation Validators
            builder.Services.AddScoped<IValidator<Appointment>, AppointmentValidator>();
            builder.Services.AddScoped<IValidator<DoctorSchedule>, DoctorScheduleValidator>();
            builder.Services.AddScoped<IValidator<Doctor>, DoctorValidator>();
            builder.Services.AddScoped<IValidator<Patient>, PatientValidator>();
            builder.Services.AddScoped<IValidator<Domain.Entities.Clinic>, ClinicValidator>();

            // Add FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next();
            });
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                // Enable detailed errors in development
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}