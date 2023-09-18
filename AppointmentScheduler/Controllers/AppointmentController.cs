using AppointmentScheduler.Services;
using AppointmentScheduler.Utilty;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppoitmentService _appoitmentService;     // for call the appointment service we create it in container so register
                                                                    // it first in progrom.cs with AddTransient which is short life time 
        public AppointmentController(IAppoitmentService appoitmentService)
        {
            _appoitmentService = appoitmentService;
        }                                                 // pass the Interface and Implimentation Class then we acces it here as a 
                                                          // dependency injection
        public IActionResult Index()
        {
            ViewBag.Duration = Helper.GetTimeDropDown();
            ViewBag.doctorDropDownList = _appoitmentService.GetDoctorList();

            ViewBag.patientDropDownList = _appoitmentService.GetPatientList();
            return View();
        }
    }
}
