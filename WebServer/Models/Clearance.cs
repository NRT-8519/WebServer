using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models
{
    [Table("clearance")]
    [PrimaryKey("id")]
    public class Clearance
    {
        [Column("id")]
        [Key]
        [Required]
        public uint? Id { get; set; }

        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

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
