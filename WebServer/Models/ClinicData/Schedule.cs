using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Models.UserData;

namespace WebServer.Models.ClinicData
{
    [Table("schedule")]
    [PrimaryKey("ScheduleId")]
    public class Schedule
    {
        [Required]
        [Key]
        [Column("schedule_id")]
        public uint? ScheduleId { get; set; }

        [Required]
        [Key]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [ForeignKey("DoctorUUID")]
        public User Doctor { get; set; }

        [Required]
        [Key]
        [Column("patient_uuid")]
        public Guid PatientUUID { get; set; }

        [Required]
        [ForeignKey("PatientUUID")]
        public User Patient { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Column("scheduled_date_time")]
        public DateTime ScheduledDateTime { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [Column("event")]
        public string Event { get; set; }

    }
}
