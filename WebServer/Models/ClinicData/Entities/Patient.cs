using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebServer.Models.UserData;

namespace WebServer.Models.ClinicData.Entities
{
    [Table("patient")]
    public class Patient : User
    {
        [Column("uuid")]
        [Required]
        public Guid PatientUUID { get; set; }

        [Required]
        [ForeignKey("PatientUUID")]
        public List<Notes> Notes { get; set; } = new List<Notes>();

        [Required]
        [ForeignKey("PatientUUID")]
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        [Required]
        [ForeignKey("PatientUUID")]
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
