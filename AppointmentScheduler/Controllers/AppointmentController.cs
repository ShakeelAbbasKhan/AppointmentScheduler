using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Services;
using AppointmentScheduler.Utilty;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace AppointmentScheduler.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppoitmentService _appoitmentService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string? loginUserId;
        private readonly string? role;
        // for call the appointment service we create it in container so register
        // it first in progrom.cs with AddTransient which is short life time 
        public AppointmentController(IAppoitmentService appoitmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appoitmentService = appoitmentService;

            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }                                                 // pass the Interface and Implimentation Class then we acces it here as a 
                                                          // dependency injection
        public IActionResult Index()
        {
            ViewBag.Duration = Helper.GetTimeDropDown();
            ViewBag.doctorDropDownList = _appoitmentService.GetDoctorList();

            ViewBag.patientDropDownList = _appoitmentService.GetPatientList();
            return View();
        }


        [HttpPost("Appointment/SaveCalendarData")]
            
        public IActionResult SaveCalendarData([FromBody] AppointmentVM model)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _appoitmentService.AddUpdateAppointment(model).Result;
                if (commonResponse.status == 2)
                {
                    commonResponse.mesesage = Helper.appointmentAdded;
                }
                if (commonResponse.status == 1)
                {
                    commonResponse.mesesage = Helper.appointmentUpdated;
                }
            }
            catch (Exception e)
            {
                commonResponse.mesesage = e.Message;
                commonResponse.status = Helper.failure_code;

            }
            return Ok(commonResponse);
        }

        [HttpGet("Appointment/GetCalendarData")]
        public async Task<IActionResult> GetCalendarData(string doctorId)
        {
            CommonResponse<AppointmentVM> commonResponse = new CommonResponse<AppointmentVM>();

            try
            {
                if (role == Helper.Patient)
                {
                    commonResponse.dataemum = await _appoitmentService.PatientsEventById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    commonResponse.dataemum = await _appoitmentService.DoctorsEventById(loginUserId);
                    commonResponse.status = Helper.success_code;
                }
                else
                {
                    commonResponse.dataemum = await _appoitmentService.DoctorsEventById(doctorId);
                    commonResponse.status = Helper.success_code;
                }
            }
            catch (Exception e)
            {
                commonResponse.mesesage = e.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        }
        [HttpGet("Appointment/GetCalendarDataById/{id}")]
        public  IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentVM> commonResponse = new CommonResponse<AppointmentVM>();

            try
            {
               
                    commonResponse.dataemum = _appoitmentService.GetById(id);
                    commonResponse.status = Helper.success_code;
              
            }
            catch (Exception e)
            {
                commonResponse.mesesage = e.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        }
    }
}
