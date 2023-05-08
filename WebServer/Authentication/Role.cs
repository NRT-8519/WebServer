using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Authentication
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Required]
        [Column("role_name")]
        public string RoleName { get; set; }
    }
}
