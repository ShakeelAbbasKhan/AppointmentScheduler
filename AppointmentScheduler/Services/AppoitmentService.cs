using AppointmentScheduler.Models;
using AppointmentScheduler.Models.ViewModels;
using AppointmentScheduler.Utilty;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace AppointmentScheduler.Services
{
    public class AppoitmentService : IAppoitmentService
    {
        private readonly ApplicationDbContext _db;
        // user Mananger
        UserManager<ApplicationUser> _userManager;
        public AppoitmentService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;

            _userManager = userManager;
            
        }

        public async Task<int> AddUpdateAppointment(AppointmentVM model)
        {

            string dateFormat = "M/d/yyyy h:mm tt"; 
            var startDate = DateTime.ParseExact(model.StartDate, dateFormat, CultureInfo.InvariantCulture);
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

       
        public async Task<List<DoctorVM>> GetDoctorList()
        {
            // one method to get dropdown using query
            //var doctors = (from user in _db.Users 
            //                join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
            //                join roles in _db.Roles.Where(x=>x.Name == Helper.Doctor) on userRoles.RoleId equals roles.Id
            //               select new DoctorVM
            //               {
            //                   Id = user.Id ,
            //                   Name = user.Name
            //               }
            //               ).ToList();
            //return doctors;
            // 2nd method to get dropdown using userManager and RoleAsyn
            var doctors = await _userManager.GetUsersInRoleAsync(Utilty.Helper.Doctor);
            var doctorsVM = doctors.Select(user => new DoctorVM()
            {
                Id = user.Id,
                Name = user.Name

            }).ToList();

            return doctorsVM;
        }

        public async Task<List<PatientVM>> GetPatientList()
        {
            var patients = await _userManager.GetUsersInRoleAsync(Utilty.Helper.Patient);
            var patientsVM = patients.Select(user => new PatientVM()
            {
                Id = user.Id,
                Name = user.Name

            }).ToList();

            return patientsVM;
        }




        public List<AppointmentVM> DoctorsEventById(string doctorId)
        {
            return _db.Appointments.Where(x => x.DoctorId == doctorId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = c.Duration,
                isDoctorApproved = c.isDoctorApproved,
            }).ToList();

          //  return appointments;

        }

        public AppointmentVM GetById(int id)
        {
           


            return _db.Appointments
               .Where(x => x.Id == id).ToList()
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
                   PatientName = _db.Users.Where(x => x.Id == c.PatientId).Select(x => x.Name).FirstOrDefault(),
                   DoctorName = _db.Users.Where(x => x.Id == c.DoctorId).Select(x => x.Name).FirstOrDefault()
               }).SingleOrDefault();
        }

        public List<AppointmentVM> PatientsEventById(string patientId)
        {
            return _db.Appointments.Where(x => x.PatientId == patientId).ToList().Select(c => new AppointmentVM()
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = c.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = c.Duration,
                isDoctorApproved = c.isDoctorApproved,
     
            }).ToList();

           // return appointments;

        }

        public async Task<int> ConfirmEvent(int id)
        {
            var appointments = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointments !=null)
            {
                appointments.isDoctorApproved = true;
                return await _db.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> DeleteEvent(int id)
        {
            var appointments = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointments != null)
            {
                _db.Appointments.Remove(appointments);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }
    }
}
