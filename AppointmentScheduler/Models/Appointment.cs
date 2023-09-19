using System.ComponentModel.DataAnnotations;

namespace AppointmentScheduler.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public bool isDoctorApproved { get; set; }
        public string AdminId { get; set; }

    }
}
