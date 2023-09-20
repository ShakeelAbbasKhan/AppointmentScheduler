using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppoitmentService
    {
        public Task<List<DoctorVM>> GetDoctorList();
        public Task<List<PatientVM>> GetPatientList();
        public Task<int> AddUpdateAppointment(AppointmentVM model);
        public List<AppointmentVM> DoctorsEventById(string doctorId);   // particular doctors have appointments and show them on calendar
        public List<AppointmentVM> PatientsEventById(string patientId);

        public AppointmentVM GetById(int id);       // to show related data in view model


    }
}
