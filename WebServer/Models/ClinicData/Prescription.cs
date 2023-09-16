using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WebServer.Models.MedicineData;
using WebServer.Models.UserData;

namespace WebServer.Models.ClinicData
{
    [Table("prescription")]
    [PrimaryKey("Id")]
    public class Prescription
    {
        [Required]
        [Key]
        [Column("id")]
        public uint? Id { get; set; }

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
        [Column("medicine_uuid")]
        public Guid MedicineUUID { get; set; }

        [Required]
        [ForeignKey("MedicineUUID")]
        public Medicine Medicine { get; set; }

        [AllowNull]
        [Column("date_prescribed")]
        public DateOnly? DatePrescribed { get; set; }

        [AllowNull]
        [Column("date_administered")]
        public DateOnly? DateAdministered { get; set; }

        [Column("prescription_notes")]
        [StringLength(100)]
        public string PrescriptionNotes { get; set; }
    }
}
