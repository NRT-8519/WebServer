using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.MedicineData
{
    [Table("company")]
    [PrimaryKey("Id")]
    public class Company
    {
        [Column("id")]
        [Key]
        public uint? Id { get; set; }

        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        [Column("name")]
        [Key]
        [Required]
        public string Name { get; set; }

        [Column("country")]
        [Required]
        public string Country { get; set; }

        [Column("city")]
        [Required]
        public string City { get; set; }

        [Column("address")]
        [Required]
        public string Address { get; set; }
    }
}
