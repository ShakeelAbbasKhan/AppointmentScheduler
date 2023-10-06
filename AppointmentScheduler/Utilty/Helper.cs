using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentScheduler.Utilty
{
    public static class Helper  // make static so we can use it through the code
    {
        public static string Admin = "Admin";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        // message to response if data, update or not

        public static string appointmentAdded = "Appointment Added Successfuly.";
        public static string appointmentUpdated = "Appointment Updated Successfuly.";
        public static string appointmentDeleted = "Appointment Deleted Successfuly.";
        public static string appointmentExists = "Appointment for select date and time already exist.";
        public static string appointmentNotExists = "Appointment Not Exists.";

        public static string appointmentConfirm = "Appointment Confrim.";
        public static string appointmentConfrimError = "Error while confriming the appointment.";

        public static string appointmentAddError = "Something went wrong, please try again.";
        public static string appointmentUpdateError = "Something went wrong, please try again.";
        public static string somethingWentWrong = "Something went wrong, please try again.";

        public static int success_code = 1;
        public static int failure_code = 0;



        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
             {
                 new SelectListItem {Value=Helper.Admin,Text = Helper.Admin}

             };

            }
            else
            {
                return new List<SelectListItem>
             {
                 new SelectListItem {Value=Helper.Patient,Text = Helper.Patient},
                 new SelectListItem {Value=Helper.Doctor,Text = Helper.Doctor}

             };
            }
        }

        public static List<SelectListItem> GetTimeDropDown()
        {
            int minute = 60;
            List<SelectListItem> duration = new List<SelectListItem>();
            for (int i = 1;i<=12;i++) { 
            duration.Add(new SelectListItem { Value = minute.ToString(),Text = i + "Hr"});
                minute = minute + 30;
                duration.Add(new SelectListItem { Value = minute.ToString(), Text = i + "Hr 30 min" });
                minute = minute + 30;
            }
            return duration;
        }
    }
}
