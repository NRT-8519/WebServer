namespace WebServer.Models.DTOs
{
    public class PrescriptionDTO
    {
        public uint? Id { get; set; }
        public string Doctor { get; set; }
        public string Patient { get; set; }
        public string Medicine { get; set; }
        public DateOnly? Prescribed { get; set; }
        public DateOnly? Administered { get; set; }
        public string Notes { get; set; }
    }
}
