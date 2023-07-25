using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.MedicineData
{
    [Table("clearance")]
    [PrimaryKey("MedicineId")]
    public class Clearance
    {
        [Column("medicine_id")]
        [Key]
        [Required]
        public uint MedicineId { get; set; }

        [Column("clearance_number")]
        [Required]
        public string ClearanceNumber { get; set; }

        [Column("begin_date")]
        [Required]
        public DateOnly BeginDate { get; set; }

        [Column("expiry_date")]
        [Required]
        public DateOnly ExpiryDate { get; set; }
    }
}
