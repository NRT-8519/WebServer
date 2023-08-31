namespace WebServer.Models.DTOs
{
    public class ScheduleDTO
    {
        public uint? Id { get; set; }
        public Guid DoctorUUID { get; set; }
        public Guid PatientUUID { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public string Event { get; set; }
    }
}
