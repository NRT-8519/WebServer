using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.ClinicData
{
    [Table("work_shift")]
    [PrimaryKey("DoctorUUID")]
    public class WorkShift
    {
        [Required]
        [Key]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [Column("work_date")]
        public DateOnly WorkDate { get; set; }

        [Required]
        [Column("shift")]
        public string Shift { get; set; }
    }
}
