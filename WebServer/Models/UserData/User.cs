using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebServer.Models.UserData.Relations;

namespace WebServer.Models.UserData
{
    [Table("user")]
    [PrimaryKey("Id")]
    public class User
    {
        /// <summary>
        /// User's ID
        /// </summary>
        [Column("id")]
        [Key]
        [Required]
        public uint? Id { get; set; }

        /// <summary>
        /// User's UUID
        /// </summary>
        [Column("uuid")]
        [Key]
        [Required]
        public Guid? UUID { get; set; }

        /// <summary>
        /// User's Username value
        /// </summary>
        [Column("username")]
        [Key]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// User's Password value. Should be encrypted.
        /// </summary>
        [Column("password")]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// List of user's emails. One user can have more than one email.
        /// </summary>
        [Required]
        [ForeignKey("UserId")]
        public List<UserEmail> Emails { get; set; } = new List<UserEmail>();

        /// <summary>
        /// User's phone numbers. One user can have more than one phone number.
        /// </summary>
        [Required]
        [ForeignKey("UserId")]
        public List<UserPhoneNumber> PhoneNumbers { get; set; } = new List<UserPhoneNumber>();

        /// <summary>
        /// Specifies whether a user's account is disabled.
        /// </summary>
        [Required]
        [Column("disabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Specifies whether a user's password has expired and needs to be changed.
        /// </summary>
        [Required]
        [Column("expired")]
        public bool IsExpired { get; set; }

        /// <summary>
        /// Specifies password expiry date.
        /// </summary>
        [Required]
        [Column("password_expiry")]
        public DateTime PasswordExpiryDate { get; set; }

        /// <summary>
        /// List of user roles. One user can have more than one role.
        /// </summary>
        [Required]
        [ForeignKey("UserId")]
        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
