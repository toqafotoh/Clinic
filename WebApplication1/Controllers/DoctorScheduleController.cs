using Clinic.Application.Interfaces.Services;
using Clinic.Application.Validators;
using Clinic.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class DoctorScheduleController : Controller
    {
        private readonly IDoctorScheduleService _scheduleService;
        private readonly IDoctorService _doctorService;
        private readonly IValidator<DoctorSchedule> _validator;

        public DoctorScheduleController(
            IDoctorScheduleService scheduleService,
            IDoctorService doctorService,
            IValidator<DoctorSchedule> validator)
        {
            _scheduleService = scheduleService;
            _doctorService = doctorService;
            _validator = validator;
        }

        // GET: DoctorSchedule
        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleService.GetAllAsync();
            return View(schedules);
        }

        // GET: DoctorSchedule/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Doctors = (await _doctorService.GetAllAsync())
                .Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = d.Name
                }).ToList();

            return View();
        }

        // POST: DoctorSchedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorSchedule schedule)
        {
            var validationResult = await _validator.ValidateAsync(schedule);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                ViewBag.Doctors = (await _doctorService.GetAllAsync())
                    .Select(d => new SelectListItem
                    {
                        Value = d.DoctorId.ToString(),
                        Text = d.Name
                    }).ToList();

                return View(schedule);
            }

            try
            {
                await _scheduleService.AddAsync(schedule);
                TempData["SuccessMessage"] = "Schedule created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error creating schedule.";
                ViewBag.Doctors = (await _doctorService.GetAllAsync())
                    .Select(d => new SelectListItem
                    {
                        Value = d.DoctorId.ToString(),
                        Text = d.Name
                    }).ToList();

                return View(schedule);
            }
        }

        // GET: DoctorSchedule/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var selectedSchedule = await _scheduleService.GetByIdAsync(id);

            if (selectedSchedule == null) return NotFound();

            ViewBag.Doctors = (await _doctorService.GetAllAsync())
                .Select(d => new SelectListItem
                {
                    Value = d.DoctorId.ToString(),
                    Text = d.Name
                }).ToList();

            return View(selectedSchedule);
        }

        // POST: DoctorSchedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorSchedule schedule)
        {
            if (id != schedule.DoctorScheduleId) return NotFound();

            var validationResult = await _validator.ValidateAsync(schedule);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                ViewBag.Doctors = (await _doctorService.GetAllAsync())
                    .Select(d => new SelectListItem
                    {
                        Value = d.DoctorId.ToString(),
                        Text = d.Name
                    }).ToList();

                return View(schedule);
            }

            try
            {
                await _scheduleService.UpdateAsync(schedule);
                TempData["SuccessMessage"] = "Schedule updated successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error updating schedule.";
                ViewBag.Doctors = (await _doctorService.GetAllAsync())
                    .Select(d => new SelectListItem
                    {
                        Value = d.DoctorId.ToString(),
                        Text = d.Name
                    }).ToList();

                return View(schedule);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: DoctorSchedule/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);

            if (schedule == null) return NotFound();
            return View(schedule);
        }

        // POST: DoctorSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _scheduleService.GetByIdAsync(id);

            if (schedule != null)
            {
                await _scheduleService.DeleteAsync(schedule);
                TempData["SuccessMessage"] = "Schedule deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}