using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.UserData.Relations
{
    [Table("user_phone_numbers")]
    [PrimaryKey("UserId")]
    public class UserPhoneNumber
    {
        [Key]
        [Required]
        [Column("user_id")]
        public uint UserId { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
