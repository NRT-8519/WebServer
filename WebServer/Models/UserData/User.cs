using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebServer.Models.UserData.Relations;

namespace WebServer.Models.UserData
{
    [Table("user")]
    [PrimaryKey("UUID")]
    public class User
    {
        /// <summary>
        /// User's ID
        /// </summary>
        [Column("id")]
        [Key]
        [JsonIgnore]
        public uint Id { get; set; }

        /// <summary>
        /// User's UUID
        /// </summary>
        [Column("uuid")]
        [Key]
        [Required]
        public Guid UUID { get; set; }

        /// <summary>
        /// User's Username value
        /// </summary>
        [Column("username")]
        [Key]
        public string Username { get; set; }

        /// <summary>
        /// User's Password value. Should be encrypted.
        /// </summary>
        [Column("password")]
        [JsonIgnore]
        public string Password { get; set; }


        /// <summary>
        /// User's personal data, such as first name, last name, date of birth etc.
        /// </summary>

        [ForeignKey("UUID")]
        public PersonalData PersonalData { get; set; }

        /// <summary>
        /// Specifies whether a user's account is disabled.
        /// </summary>
        [Column("disabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Specifies whether a user's password has expired and needs to be changed.
        /// </summary>
        [Column("expired")]
        public bool IsExpired { get; set; }

        /// <summary>
        /// Specifies password expiry date.
        /// </summary>
        [Column("password_expiry")]
        public DateTime PasswordExpiryDate { get; set; }

        /// <summary>
        /// List of user roles. One user can have only one role.
        /// </summary>
        [ForeignKey("UserUUID")]
        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
