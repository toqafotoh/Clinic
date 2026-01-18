using Clinic.Application.Interfaces.Services;
using Clinic.Application.Validators;
using Clinic.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClinicEntity = Clinic.Domain.Entities.Clinic;
namespace WebApplication1
{
    public class ClinicController : Controller
    {
        private readonly IClinicService _clinicService;
        private readonly IValidator<ClinicEntity> _validator;

        public ClinicController(IClinicService clinicService, IValidator<ClinicEntity> validator)
        {
            _clinicService = clinicService;
            _validator = validator;
        }

        // GET: Clinic
        public async Task<IActionResult> Index()
        {
            var clinics = await _clinicService.GetAllAsync();
            return View(clinics);
        }

        // GET: Clinic/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic == null) return NotFound();
            return View(clinic);
        }

        // GET: Clinic/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clinic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClinicEntity clinic)
        {
            var validationResult = await _validator.ValidateAsync(clinic);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(clinic);
            }

            try
            {
                await _clinicService.AddAsync(clinic);
                TempData["SuccessMessage"] = "Clinic created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error creating clinic.";
                return View(clinic);
            }
        }

        // GET: Clinic/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic == null) return NotFound();
            return View(clinic);
        }

        // POST: Clinic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClinicEntity clinic)
        {
            if (id != clinic.ClinicId) return NotFound();

            var validationResult = await _validator.ValidateAsync(clinic);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                return View(clinic);
            }

            try
            {
                await _clinicService.UpdateAsync(clinic);
                TempData["SuccessMessage"] = "Clinic updated successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error updating clinic.";
                return View(clinic);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clinic/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic == null) return NotFound();
            return View(clinic);
        }

        // POST: Clinic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinic = await _clinicService.GetByIdAsync(id);
            if (clinic != null)
            {
                await _clinicService.DeleteAsync(clinic);
                TempData["SuccessMessage"] = "Clinic deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}