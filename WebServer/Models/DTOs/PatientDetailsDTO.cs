namespace WebServer.Models.DTOs
{
    public class PatientDetailsDTO : UserDetailsDTO
    {
        public UserBasicDTO AssignedDoctor { get; set; }
    }
}
