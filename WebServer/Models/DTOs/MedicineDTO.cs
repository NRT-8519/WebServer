namespace WebServer.Models.DTOs
{
    public class MedicineDTO
    {
        public Guid? UUID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Dosage { get; set; }
        public string DosageType { get; set; }
        public string EAN { get; set; }
        public string ATC { get; set; }
        public string UniqueClassification { get; set; }
        public string INN { get; set; }
        public string PrescriptionType { get; set; }
        public CompanyDTO Company { get; set; }
        public IssuerDTO Issuer { get; set; }
        public ClearanceDTO Clearance { get; set; } = new();
    }
}
