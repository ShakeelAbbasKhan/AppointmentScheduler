using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppoitmentService
    {
        public List<DoctorVM> GetDoctorList();
        public List<PatientVM> GetPatientList();
        public Task<int> AddUpdateAppointment(AppointmentVM model);
        public Task<AppointmentVM> DoctorsEventById(string doctorId);
        public Task<AppointmentVM> PatientsEventById(string patientId);

        public AppointmentVM GetById(int id);


    }
}
