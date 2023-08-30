using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Column("date_prescribed")]
        public DateOnly? DatePrescribed { get; set; }

        [Column("date_administered")]
        public DateOnly? DateAdministered { get; set; }

        [Column("prescription_notes")]
        public string PrescriptionNotes { get; set; }
    }
}
