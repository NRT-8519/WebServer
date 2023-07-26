using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebServer.Models.UserData;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.ClinicData.Entities
{
    [Table("doctor")]
    public class Doctor : User
    {
        [Column("uuid")]
        [Required]
        public Guid DoctorUUID { get; set; }

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<Notes> Notes {  get; set; } = new List<Notes>();

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<TimeOff> TimeOffs { get; set; } = new List<TimeOff>();

        [Required]
        [ForeignKey("DoctorUUID")]
        public List<WorkShift> WorkShifts { get; set; } = new List<WorkShift>();
    }
}
