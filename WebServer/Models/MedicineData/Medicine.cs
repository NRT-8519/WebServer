using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Models.MedicineData.Relations;

namespace WebServer.Models.MedicineData
{
    [Table("medicine")]
    public class Medicine
    {
        [Column("id")]
        [Key]
        [Required]
        public uint? Id { get; set; }

        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        [Column("name")]
        [Key]
        [Required]
        public string Name { get; set; }

        [Column("type")]
        [Required]
        public EnumMedicineType Type { get; set; }

        [Column("dosage")]
        [Required]
        public string Dosage { get; set; }

        [Column("dosage_type")]
        [Required]
        public string DosageType { get; set; }

        [Column("ean_code")]
        [Required]
        public string EAN { get; set; }

        [Column("atc")]
        [Required]
        public string ATC { get; set; }

        [Column("unique_classification")]
        public string UniqueClassification { get; set; }

        [Column("inn")]
        [Required]
        public string INN { get; set; }

        [Column("prescription_type")]
        [Required]
        public EnumPrescriptionType PrescriptionType { get; set; }

        [Required]
        [ForeignKey("MedicineId")]
        public MedicineCompany Company { get; set; }

        [Required]
        [ForeignKey("MedicineId")]
        public MedicineIssuer Issuer { get; set; }

        [Required]
        [ForeignKey("MedicineId")]
        public MedicineClearance Clearance { get; set; }
    }
}
