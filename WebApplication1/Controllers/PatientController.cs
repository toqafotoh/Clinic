using Clinic.Application.Interfaces.Services;
using Clinic.Application.Validators;
using Clinic.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IValidator<Patient> _validator;

        public PatientController(IPatientService patientService, IValidator<Patient> validator)
        {
            _patientService = patientService;
            _validator = validator;
        }

        // GET: Patient
        public async Task<IActionResult> Index()
        {
            var patients = await _patientService.GetAllAsync();
            return View(patients);
        }

        // GET: Patient/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient)
        {
            var validationResult = await _validator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(patient);
            }

            try
            {
                await _patientService.AddAsync(patient);
                TempData["SuccessMessage"] = "Patient created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error creating patient.";
                return View(patient);
            }
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Patient patient)
        {
            if (id != patient.PatientId) return NotFound();

            var validationResult = await _validator.ValidateAsync(patient);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(patient);
            }

            try
            {
                await _patientService.UpdateAsync(patient);
                TempData["SuccessMessage"] = "Patient updated successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error updating patient.";
                return View(patient);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient != null)
            {
                await _patientService.DeleteAsync(patient);
                TempData["SuccessMessage"] = "Patient deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}