namespace WebServer.Models.DTOs
{
    public class DoctorDetailsDTO
    {
        public Guid UUID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public char Gender { get; set; }
        public string SSN { get; set; }
        public DateTime PasswordExpiryDate { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsExpired { get; set; }
        public string AreaOfExpertise { get; set; }
        public int RoomNumber { get; set; }
        public List<UserBasicDTO> Patients { get; set; } = new();
    }
}
