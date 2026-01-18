using Clinic.Application.Common;
using Clinic.Application.Interfaces.Services;
using Clinic.Application.Validators;
using Clinic.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.Web
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IValidator<Appointment> _validator;

        public AppointmentController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IPatientService patientService,
            IValidator<Appointment> validator)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _patientService = patientService;
            _validator = validator;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAsync();
            return View(appointments);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // GET: Appointment/CreateSlot
        public async Task<IActionResult> CreateSlot()
        {
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View();
        }

        // POST: Appointment/CreateSlot
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlot(int doctorId, DateTime date, TimeSpan startTime)
        {
            var result = await _appointmentService.CreateAppointmentSlotAsync(doctorId, date, startTime);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Message;
            ViewBag.Doctors = await _doctorService.GetAllAsync();
            return View();
        }

        // GET: Appointment/Book/5
        public async Task<IActionResult> Book(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null) return NotFound();

            ViewBag.Patients = await _patientService.GetAllAsync();
            return View(appointment);
        }

        // POST: Appointment/Book/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(int id, int patientId)
        {
            var result = await _appointmentService.BookAppointmentAsync(id, patientId);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Message;
            var appointment = await _appointmentService.GetByIdAsync(id);
            ViewBag.Patients = await _patientService.GetAllAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointment/GetFreeSlots
        public async Task<IActionResult> GetFreeSlots(int doctorId, DateTime date)
        {
            var slots = await _appointmentService.GetFreeSlotsAsync(doctorId, date);
            return Json(slots);
        }

        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null) return NotFound();

            // Convert to SelectListItem
            var doctors = await _doctorService.GetAllAsync();
            ViewBag.Doctors = doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorId.ToString(),
                Text = d.Name
            }).ToList();

            var patients = await _patientService.GetAllAsync();
            ViewBag.Patients = patients.Select(p => new SelectListItem
            {
                Value = p.PatientId.ToString(),
                Text = p.Name
            }).ToList();

            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId) return NotFound();

            var validationResult = await _validator.ValidateAsync(appointment);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                // Convert to SelectListItem when validation fails
                var doctors = await _doctorService.GetAllAsync();
                ViewBag.Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = d.Name
                }).ToList();

                var patients = await _patientService.GetAllAsync();
                ViewBag.Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.PatientId.ToString(),
                    Text = p.Name
                }).ToList();

                return View(appointment);
            }

            try
            {
                await _appointmentService.UpdateAsync(appointment);
                TempData["SuccessMessage"] = "Appointment updated successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error updating appointment.";

                // Convert to SelectListItem when error occurs
                var doctors = await _doctorService.GetAllAsync();
                ViewBag.Doctors = doctors.Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = d.Name
                }).ToList();

                var patients = await _patientService.GetAllAsync();
                ViewBag.Patients = patients.Select(p => new SelectListItem
                {
                    Value = p.PatientId.ToString(),
                    Text = p.Name
                }).ToList();

                return View(appointment);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment != null)
            {
                await _appointmentService.DeleteAsync(appointment);
                TempData["SuccessMessage"] = "Appointment deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: API endpoints for JavaScript
        [HttpGet]
        public async Task<IActionResult> GetAllDoctorsJson()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Json(doctors.Select(d => new {
                id = d.DoctorId,
                name = d.Name
            }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatientsJson()
        {
            var patients = await _patientService.GetAllAsync();
            return Json(patients.Select(p => new {
                id = p.PatientId,
                name = p.Name
            }));
        }
    }
}