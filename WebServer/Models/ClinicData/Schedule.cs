using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.ClinicData
{
    [Table("schedule")]
    [PrimaryKey("ScheduleId")]
    public class Schedule
    {
        [Required]
        [Key]
        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Required]
        [Key]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [Key]
        [Column("patient_uuid")]
        public Guid PatientUUID { get; set; }

        [Required]
        [Column("scheduled_date_time")]
        public DateTime ScheduledDateTime { get; set; }

        [Required]
        [Column("event")]
        public string Event { get; set; }

    }
}
