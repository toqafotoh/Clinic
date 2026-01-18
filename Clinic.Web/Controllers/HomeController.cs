using Clinic.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Clinic.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;

        public HomeController(IDoctorService doctorService, IPatientService patientService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
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
    }
}