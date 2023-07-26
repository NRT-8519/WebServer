using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.ClinicData
{
    [Table("prescription")]
    [PrimaryKey("Id")]
    public class Prescription
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
        [Key]
        [Column("medicine_uuid")]
        public Guid MedicineUUID { get; set; }

        [Column("date_prescribed")]
        public DateOnly DatePrescribed { get; set; }

        [Column("date_administered")]
        public DateOnly DateAdministered { get; set; }

        [Column("prescription_notes")]
        public string PrescriptionNotes { get; set; }
    }
}
