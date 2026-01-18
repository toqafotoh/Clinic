using Clinic.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public HomeController(
            IDoctorService doctorService,
            IPatientService patientService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> BookAppointment()
        {
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            ViewBag.Patients = await _patientService.GetAllAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableAppointments(int doctorId, string date)
        {
            if (doctorId == 0 || string.IsNullOrEmpty(date))
            {
                return Json(new { success = false, message = "Invalid parameters" });
            }

            if (!DateTime.TryParse(date, out DateTime appointmentDate))
            {
                return Json(new { success = false, message = "Invalid date format" });
            }

            var appointments = await _appointmentService.GetAvailableAppointmentsAsync(doctorId, appointmentDate);

            var result = appointments.Select(a => new
            {
                id = a.AppointmentId,
                time = a.StartTime.ToString(@"hh\:mm"),
                displayText = a.StartTime.ToString(@"hh\:mm")
            }).ToList();

            return Json(new { success = true, appointments = result });
        }
    }
}