using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebServer.Authentication;

namespace WebServer.Models.UserData.Relations
{
    [Table("user_roles")]
    [PrimaryKey("UserUUID")]
    public class UserRole
    {
        [Key]
        [Required]
        [Column("user_uuid")]
        [JsonIgnore]
        public Guid UserUUID { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; }
    }
}
