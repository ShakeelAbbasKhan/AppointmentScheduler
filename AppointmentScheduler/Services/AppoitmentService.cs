using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utilty;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AppointmentScheduler.Services
{
    public class AppoitmentService : IAppoitmentService
    {
        private readonly ApplicationDbContext _db;
        public AppoitmentService(ApplicationDbContext db)
        {
            _db = db;
            
        }

        public async Task<int> AddUpdateAppointment(AppointmentVM model)
        {

            // Define the expected DateTime format for 'model.StartDate'
            string dateFormat = "M/d/yyyy h:mm tt"; // Example format, adjust as needed

            // Attempt to parse 'model.StartDate' using the specified format
            var startDate = DateTime.ParseExact(model.StartDate, dateFormat, CultureInfo.InvariantCulture);

            // Calculate 'endDate' by adding the specified duration in minutes
            var endDate = startDate.AddMinutes(Convert.ToDouble(model.Duration));

            //var startDate = DateTime.Parse(model.StartDate);
            //var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));

            if(model != null && model.Id>0)
            {
                //update
                return 1;
            }
            else
            {
                // create

                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    isDoctorApproved = false,
                     AdminId = "1",
                };
                _db.Appointments.Add(appointment);

                await _db.SaveChangesAsync();
                return 2;
            }
        }

        public async Task<AppointmentVM> DoctorsEventById(string doctorId)
        {
            var appointment = await _db.Appointments
                .Where(x => x.DoctorId == doctorId)
                .Select(c => new AppointmentVM()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Duration = c.Duration,
                    isDoctorApproved = c.isDoctorApproved
                })
                .FirstOrDefaultAsync();

            return appointment;
        }

        public AppointmentVM GetById(int id)
        {
            var appointment = _db.Appointments
               .Where(x => x.Id == id)
               .Select(c => new AppointmentVM()
               {
                   Id = c.Id,
                   Title = c.Title,
                   Description = c.Description,
                   StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                   EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                   Duration = c.Duration,
                   isDoctorApproved = c.isDoctorApproved,
                   PatientId = c.PatientId,
                   DoctorId = c.DoctorId,
                   PatientName = _db.Users.Where(x=> x.Id == c.PatientId).Select(x=> x.Name).FirstOrDefault(),
                   DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
               }).SingleOrDefault();

            return appointment;
        }

        public List<DoctorVM> GetDoctorList()
        {
            var doctors = (from user in _db.Users 
                            join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                            join roles in _db.Roles.Where(x=>x.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
                           select new DoctorVM
                           {
                               Id = user.Id ,
                               Name = user.Name
                           }
                           ).ToList();
            return doctors;
        }

        public List<PatientVM> GetPatientList()
        {
            var patient = (from user in _db.Users
                           join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                           join roles in _db.Roles.Where(x => x.Name == Helper.Patient) on userRoles.RoleId equals roles.Id
                           select new PatientVM
                           {
                               Id = user.Id,
                               Name = user.Name
                           }).ToList();
            return patient;
        }

        public async Task<AppointmentVM> PatientsEventById(string patientId)
        {
            var appointment = await _db.Appointments
                .Where(x => x.PatientId == patientId)
                .Select(c => new AppointmentVM()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Duration = c.Duration,
                    isDoctorApproved = c.isDoctorApproved
                })
                .FirstOrDefaultAsync();

            return appointment;
        }

    }
}
