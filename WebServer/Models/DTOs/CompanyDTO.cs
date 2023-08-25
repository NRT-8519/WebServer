namespace WebServer.Models.DTOs
{
    public class CompanyDTO
    {
        public Guid? UUID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
