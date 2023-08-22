using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        public string Role { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }

        [Column("ssn")]
        public string SSN { get; set; }

        [Column("gender")]
        public char Gender { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
