namespace WebServer.Models.DTOs
{
    public class UserDTO
    {
        public Guid UUID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string SSN { get; set; }
        public char Gender { get; set; }
        public List<string> Emails { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public string Role { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsExpired { get; set; }
        public DateTime PasswordExpiry { get; set; }
    }
}
