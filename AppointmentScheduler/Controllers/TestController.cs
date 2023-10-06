using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduler.Controllers
{
    public class TestController : Controller
    {
        string connectionString = "Server=DESKTOP-MTRJH4J\\SQLEXPRESS;Database=AppointmentScheduler;Trusted_Connection=True;TrustServerCertificate=True;";
        public IActionResult Index()
        {
            return View();
        }
    }
}
 