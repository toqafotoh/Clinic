using Clinic.Application.Interfaces.Services;
using Clinic.Application.Validators;
using Clinic.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.WebUI.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IClinicService _clinicService;
        private readonly IValidator<Doctor> _validator;

        public DoctorController(
            IDoctorService doctorService,
            IClinicService clinicService,
            IValidator<Doctor> validator)
        {
            _doctorService = doctorService;
            _clinicService = clinicService;
            _validator = validator;
        }

        // GET: Doctor
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorService.GetAllAsync();
            return View(doctors);
        }

        // GET: Doctor/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // GET: Doctor/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Clinics = await _clinicService.GetAllAsync();
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {
            var validationResult = await _validator.ValidateAsync(doctor);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                ViewBag.Clinics = await _clinicService.GetAllAsync();
                return View(doctor);
            }

            try
            {
                await _doctorService.AddAsync(doctor);
                TempData["SuccessMessage"] = "Doctor created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error creating doctor.";
                ViewBag.Clinics = await _clinicService.GetAllAsync();
                return View(doctor);
            }
        }

        // GET: Doctor/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();

            ViewBag.Clinics = await _clinicService.GetAllAsync();
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.DoctorId) return NotFound();

            var validationResult = await _validator.ValidateAsync(doctor);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                ViewBag.Clinics = await _clinicService.GetAllAsync();
                return View(doctor);
            }

            try
            {
                await _doctorService.UpdateAsync(doctor);
                TempData["SuccessMessage"] = "Doctor updated successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error updating doctor.";
                ViewBag.Clinics = await _clinicService.GetAllAsync();
                return View(doctor);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor != null)
            {
                await _doctorService.DeleteAsync(doctor);
                TempData["SuccessMessage"] = "Doctor deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}