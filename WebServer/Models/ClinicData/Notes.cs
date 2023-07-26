using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [Column("doctor_uuid")]
        public Guid DoctorUUID { get; set; }

        [Required]
        [Key]
        [Column("patient_uuid")]
        public Guid PatientUUID { get; set; }

        [Required]
        [Column("note")]
        public string Note { get; set; }
    }
}
