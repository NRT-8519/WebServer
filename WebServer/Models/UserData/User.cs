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
        [StringLength(30)]
        public string Username { get; set; }

        /// <summary>
        /// User's Password value. Should be encrypted.
        /// </summary>
        [Column("password")]
        [JsonIgnore]
        [StringLength(45)]
        public string Password { get; set; }

        /// <summary>
        /// Specifies whether a user's account is disabled.
        /// </summary>
        [Column("disabled")]
        [Required]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Specifies whether a user's password has expired and needs to be changed.
        /// </summary>
        [Column("expired")]
        [Required]
        public bool IsExpired { get; set; }

        /// <summary>
        /// Specifies password expiry date.
        /// </summary>
        [Column("password_expiry")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime PasswordExpiryDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [Column("first_name")]
        [StringLength(30)]
        [Required]
        public string FirstName { get; set; }

        [Column("middle_name")]
        [StringLength(30)]
        [Required]
        public string MiddleName { get; set; }

        [Column("last_name")]
        [StringLength(30)]
        [Required]
        public string LastName { get; set; }

        [Column("title")]
        [StringLength(50)]
        [Required]
        public string Title { get; set; }

        [Column("date_of_birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Column("ssn")]
        [RegularExpression(@"[0-9]{13}")]
        [Required]
        public string SSN { get; set; }

        [Column("gender")]
        [RegularExpression(@"(M|F){1}")]
        [Required]
        public char Gender { get; set; }

        [Column("email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Column("phone_number")]
        [RegularExpression(@"\+[1-9][0-9]{1,2}[0-9]{8,9}$")]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
