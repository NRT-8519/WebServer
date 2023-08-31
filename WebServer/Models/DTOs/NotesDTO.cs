namespace WebServer.Models.DTOs
{
    public class NotesDTO
    {
        public uint Id { get; set; }
        public Guid DoctorUUID { get; set; }
        public Guid PatientUUID { get; set; }
        public string NoteTitle { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }

    }
}
