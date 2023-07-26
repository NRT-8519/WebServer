using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebServer.Models.UserData.Relations
{
    [Table("user_emails")]
    [PrimaryKey("UserUUID")]
    public class UserEmail
    {
        [Required]
        [Column("user_uuid")]
        [JsonIgnore]
        public Guid UserUUID{ get; set; }

        [Required]
        [Column("email")]
        public string Email { get; set; }
    }
}
