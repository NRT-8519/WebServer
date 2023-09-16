using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.MedicineData
{
    [Table("company")]
    [PrimaryKey("UUID")]
    public class Company
    {
        [Column("id")]
        public uint? Id { get; set; }

        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        [Column("name")]
        [Key]
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Column("country")]
        [Required]
        [StringLength(36)]
        public string Country { get; set; }

        [Column("city")]
        [StringLength(100)]
        [Required]
        public string City { get; set; }

        [Column("address")]
        [StringLength(100)]
        [Required]
        public string Address { get; set; }
    }
}
