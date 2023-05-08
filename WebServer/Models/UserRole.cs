using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Authentication;

namespace WebServer.Models
{
    [Table("user_roles")]
    [PrimaryKey("UserId")]
    public class UserRole
    {
        [Key]
        [Required]
        [Column("user_id")]
        public uint UserId { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; }
    }
}
