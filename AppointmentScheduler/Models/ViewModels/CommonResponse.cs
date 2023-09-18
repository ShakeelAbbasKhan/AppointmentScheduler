namespace AppointmentScheduler.Models.ViewModels
{
    public class CommonResponse<T>
    {
        public int status { get; set; }
        public string mesesage { get; set; }
        public T dataemum{ get; set; }


    }
}
