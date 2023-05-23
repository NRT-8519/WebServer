using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebServer.Models.MedicineData.Relations
{
    [Table("medicine_issuer")]
    [PrimaryKey("MedicineId")]
    public class MedicineIssuer
    {
        [Key]
        [Required]
        [Column("medicine_id")]
        public uint MedicineId { get; set; }

        [Key]
        [Required]
        [Column("issuer_id")]
        public uint IssuerId { get; set; }
    }
}
