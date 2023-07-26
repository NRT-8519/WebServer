using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebServer.Models.UserData.Relations
{
    [Table("user_phone_numbers")]
    [PrimaryKey("UserUUID")]
    public class UserPhoneNumber
    {
        [Key]
        [Required]
        [Column("user_uuid")]
        [JsonIgnore]
        public Guid UserUUID { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
