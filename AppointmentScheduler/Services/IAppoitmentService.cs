using AppointmentScheduler.Models.ViewModels;

namespace AppointmentScheduler.Services
{
    public interface IAppoitmentService
    {
        public List<DoctorVM> GetDoctorList();
        public List<PatientVM> GetPatientList();
        public Task<int> AddUpdateAppointment(AppointmentVM model);

    }
}
