using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models
{
    [Table("medicine")]
    public class Medicine
    {
        public uint? Id { get; set; }
        public Guid? UUID { get; set; }
        public string Name { get; set; }
        public EnumType Type { get; set; }
        public string Dosage { get; set; }
        public string DosageType { get; set; }
        public string EAN { get; set; }
        public string ATC { get; set; }
        public string UniqueClassification { get; set; }
        public string INN { get; set; }
        public EnumPrescriptionType PrescriptionType { get; set; }
        public Company Company { get; set; }
        public Issuer Issuer { get; set; }
        public Clearance Clearance { get; set; }
    }
}
