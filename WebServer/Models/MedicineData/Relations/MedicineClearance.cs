using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebServer.Models.MedicineData.Relations
{
    [Table("medicine_clearance")]
    [PrimaryKey("MedicineId")]
    public class MedicineClearance
    {
        [Key]
        [Required]
        [Column("medicine_id")]
        public uint MedicineId { get; set; }

        [Key]
        [Required]
        [Column("clearance_id")]
        public uint ClearanceId { get; set; }
    }
}
