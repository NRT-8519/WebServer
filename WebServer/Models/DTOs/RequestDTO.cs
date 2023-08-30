namespace WebServer.Models.DTOs
{
    public class RequestDTO
    {
        public Guid UUID { get; set; }
        public string Patient { get; set; }
        public string Doctor { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
