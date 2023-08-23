namespace WebServer.Models.DTOs
{
    public class DoctorDetailsDTO : UserDetailsDTO
    {
        public string AreaOfExpertise { get; set; }
        public int RoomNumber { get; set; }
        public List<UserBasicDTO> Patients { get; set; } = new();
    }
}
