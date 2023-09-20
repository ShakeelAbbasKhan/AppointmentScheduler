using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Services;
using AppointmentScheduler.Utilty;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AppointmentScheduler.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppoitmentService _appoitmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string? loginUserId;
        private readonly string? role;

        public AppointmentApiController(IAppoitmentService appoitmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appoitmentService = appoitmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        }                                                 // pass the Interface and Implimentation Class then we acces it here as a 

        [HttpPost("SaveCalendarData")]

        public IActionResult SaveCalendarData([FromBody] AppointmentVM model)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {
                commonResponse.status = _appoitmentService.AddUpdateAppointment(model).Result;
                if(commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }
            }
            catch(Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;

            }
            return Ok(commonResponse);
        }

        //public enum DataEnum {a,b,c }
    }
}
