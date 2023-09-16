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
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Column("type")]
        [Required]
        [StringLength(20)]
        public string Type { get; set; }

        [Column("dosage")]
        [StringLength(100)]
        [Required]
        public string Dosage { get; set; }

        [Column("dosage_type")]
        [StringLength(100)]
        [Required]
        public string DosageType { get; set; }

        [Column("ean_code")]
        [RegularExpression(@"^[0-9]{13}$")]
        [Required]
        public string EAN { get; set; }

        [Column("atc")]
        [RegularExpression(@"(^[ABCDGHJLMNPRSV][0-1][0-9][A-Z]{2}[0-9]{2})?")]
        [Required]
        public string ATC { get; set; }

        [Column("unique_classification")]
        [StringLength(7)]
        public string UniqueClassification { get; set; }

        [Column("inn")]
        [StringLength(1000)]
        [Required]
        public string INN { get; set; }

        [Column("prescription_type")]
        [StringLength(3)]
        [Required]
        public string PrescriptionType { get; set; }

        [Column("company_uuid")]
        [Required]
        public Guid? CompanyUUID { get; set; }

        [Required]
        [ForeignKey("CompanyUUID")]
        public Company Company { get; set; }

        [Column("issuer_uuid")]
        [Required]
        public Guid? IssuerUUID { get; set; }
        [Required]
        [ForeignKey("IssuerUUID")]
        public Issuer Issuer { get; set; }

        [Column("clearance_uuid")]
        [Required]
        public Guid? ClearanceUUID { get; set; }

        [Required]
        [ForeignKey("ClearanceUUID")]
        public Clearance Clearance { get; set; }
    }
}
