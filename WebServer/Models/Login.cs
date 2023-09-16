using System.ComponentModel.DataAnnotations;

namespace WebServer.Models
{
    public class Login
    {
        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Username { get; set; }

        [Required]
        [StringLength(45, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
