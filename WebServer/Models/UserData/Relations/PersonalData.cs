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
        public string FirstName { get; set; }

        [Required]
        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Required]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Required]
        [Column("date_of_birth")]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Column("ssn")]
        public string SSN { get; set; }

        [Required]
        [Column("gender")]
        public char Gender { get; set; }
    }
}
