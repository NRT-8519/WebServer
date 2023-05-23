using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.MedicineData.Relations
{
    [Table("medicine_company")]
    [PrimaryKey("MedicineId")]
    public class MedicineCompany
    {
        [Key]
        [Required]
        [Column("medicine_id")]
        public uint MedicineId { get; set; }

        [Key]
        [Required]
        [Column("company_id")]
        public uint CompanyId { get; set; }
    }
}
