namespace WebServer.Models.DTOs
{
    public class PatientBasicDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Guid UUID { get; set; }
        public DoctorBasicDTO AssignedDoctor { get; set; }
    }
}
