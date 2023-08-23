namespace WebServer.Models.DTOs
{
    public class UserBasicDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UUID { get; set; }
    }
}
