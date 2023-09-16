using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.MedicineData
{
    [Table("clearance")]
    [PrimaryKey("UUID")]
    public class Clearance
    {
        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        [Column("clearance_number")]
        [RegularExpression(@"515-01-0[0-9]{4}-[0-9]{2}-[0-9]{3}"]
        [Required]
        public string ClearanceNumber { get; set; }

        [Column("begin_date")]
        [DataType(DataType.Date)]
        [Required]
        public DateOnly BeginDate { get; set; }

        [Column("expiry_date")]
        [DataType(DataType.Date)]
        [Required]
        public DateOnly ExpiryDate { get; set; }
    }
}
