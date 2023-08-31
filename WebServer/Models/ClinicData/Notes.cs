using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Models.UserData;

namespace WebServer.Models.ClinicData
{
    [Table("note")]
    [PrimaryKey("Id")]
    public class Notes
    {
        [Required]
        [Key]
        [Column("id")]
        public uint Id { get; set; }

        [Required]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [ForeignKey("DoctorUUID")]
        public User Doctor { get; set; }
        [Required]
        [Column("patient_uuid")]
        public Guid PatientUUID { get; set; }

        [Required]
        [ForeignKey("PatientUUID")]
        public User Patient { get; set; }

        [Required]
        [Column("note_title")]
        public string NoteTitle { get; set; }

        [Required]
        [Column("note")]
        public string Note { get; set; }

        [Required]
        [Column("note_date")]
        public DateTime NoteDate { get; set; }
    }
}
