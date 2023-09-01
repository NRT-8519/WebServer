namespace WebServer.Models.DTOs
{
    public class ReportDTO
    {
        public Guid? UUID { get; set; }
        public Guid? ReportedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
