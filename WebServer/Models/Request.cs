using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Models.UserData;

namespace WebServer.Models
{
    [Table("request")]
    [PrimaryKey("UUID")]
    public class Request
    {
        [Column("uuid")]
        [Key]
        [Required]
        public Guid UUID { get; set; }

        [Required]
        [Column("patient_uuid")]
        public Guid PatientUUID { get; set; }

        [Required]
        [ForeignKey("PatientUUID")]
        public User Patient { get; set; }

        [Required]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [ForeignKey("DoctorUUID")]
        public User Doctor { get; set; }

        [Required]
        [Column("title")]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Column("description")]
        [StringLength(100, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        [Column("type")]
        public string Type { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }

        [Required]
        [Column("reason")]
        [StringLength(100, MinimumLength = 2)]
        public string Reason { get; set; }

        [Required]
        [Column("request_date")]
        [DataType(DataType.DateTime)]
        public DateTime RequestDate { get; set; }
    }
}
