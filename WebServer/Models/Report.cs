using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Models.UserData;

namespace WebServer.Models
{
    [Table("report")]
    [PrimaryKey("UUID")]
    public class Report
    {
        [Required]
        [Column("uuid")]
        [Key]
        public Guid? UUID { get; set; }

        [Required]
        [Column("user_uuid")]
        public Guid? UserUUID { get; set; }

        [Required]
        [ForeignKey("UserUUID")]
        public User User { get; set; }

        [Required]
        [Column("title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Column("description")]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Column("date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}
