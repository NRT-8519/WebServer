using System.Diagnostics.CodeAnalysis;

namespace WebServer.Models
{
    public class User
    {
        [DisallowNull]
        private string username, email, password;
        
        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
    }
}
