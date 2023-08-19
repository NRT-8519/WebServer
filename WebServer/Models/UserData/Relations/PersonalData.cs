using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebServer.Models.UserData.Relations
{
    [Table("personal_data")]
    [PrimaryKey("UUID")]
    public class PersonalData
    {
        [Key]
        [Required]
        [Column("uuid")]
        public Guid UUID { get; set; }

        [Required]
        [Column ("first_name")]
        [MinLength(2)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [Column("middle_name")]
        [MinLength(2)]
        [MaxLength(30)]
        public string MiddleName { get; set; }

        [Required]
        [Column("last_name")]
        [MinLength(2)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Column("title")]
        [MinLength(2)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [Column("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Column("ssn")]
        [MaxLength(13)]
        public string SSN { get; set; }

        [Required]
        [Column("gender")]
        public char Gender { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(50)]
        [MinLength(5)]
        public string Email { get; set; }

        [Required]
        [Column("phone_number")]
        [MaxLength(13)]
        [MinLength(9)]
        public string PhoneNumber { get; set; }
    }
}
