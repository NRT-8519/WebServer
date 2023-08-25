using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.MedicineData
{
    [Table("medicine")]
    [PrimaryKey("UUID")]
    public class Medicine
    {
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
        public string Type { get; set; }

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
        public string PrescriptionType { get; set; }

        [Column("company_id")]
        [Required]
        public uint? CompanyId { get; set; }

        [Required]
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        [Column("issuer_id")]
        [Required]
        public uint? IssuerId { get; set; }
        [Required]
        [ForeignKey("IssuerId")]
        public Issuer Issuer { get; set; }

        [Column("clearance_uuid")]
        [Required]
        public Guid? ClearanceUUID { get; set; }

        [Required]
        [ForeignKey("ClearanceUUID")]
        public Clearance Clearance { get; set; }
    }
}
