using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.ClinicData
{
    [Table("time_off")]
    [PrimaryKey("DoctorUUID")]
    public class TimeOff
    {
        [Required]
        [Key]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [Column("date_from")]
        public DateOnly DateFrom { get; set; }

        [Required]
        [Column("date_to")]
        public DateOnly DateTo { get; set; }
    }
}
