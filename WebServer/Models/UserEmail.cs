using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models
{
    [Table("user_emails")]
    [PrimaryKey("UserId")]
    public class UserEmail
    {
        [Key]
        [Required]
        [Column("user_id")]
        public uint UserId { get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }
    }
}
