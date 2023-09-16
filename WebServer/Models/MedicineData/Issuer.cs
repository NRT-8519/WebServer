using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Models.MedicineData
{
    [Table("issuer")]
    [PrimaryKey("UUID")]
    public class Issuer
    {
        [Column("id")]
        public uint? Id { get; set; }

        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        [Column("name")]
        [Key]
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Column("city")]
        [StringLength(36)]
        [Required]
        public string City { get; set; }

        [Column("area")]
        [StringLength(36)]
        [Required]
        public string Area { get; set; }
    }
}
