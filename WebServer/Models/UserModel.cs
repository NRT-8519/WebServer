using System.Diagnostics.CodeAnalysis;
using WebServer.Auth;

namespace WebServer.Models
{
    public class UserModel
    {
        [DisallowNull]
        private string username, email, password;

        [DisallowNull]
        private bool isDisabled, isExpired;

        [DisallowNull]
        private DateTime passwordExpiryDate;

        private List<string> roles = new();

        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }

        public bool IsDisabled { get => isDisabled; set => isDisabled = value; }
        public bool IsExpired { get => isExpired; set => isExpired = value; }

        public DateTime PasswordExpiryDate { get => passwordExpiryDate; set => passwordExpiryDate = value; }

        public List<string> Roles { get => roles; set => roles = value; }
        
        /// <summary>
        ///    Constructor that sets the default initial values
        /// </summary>
        private UserModel()
        {
            this.IsDisabled = false;
            this.IsExpired = false;
            this.PasswordExpiryDate = DateTime.Now.AddDays(180);
            this.Roles.Add(Role.ROLE_USER);
        }

        public UserModel(string username, string email, string password): this()
        {
            this.Username = username;
            this.Email = email;
            this.Password = password;
        }

        public UserModel(string username, string email, string password, bool isDisabled, bool isExpired) : this(username, email, password)
        {
            this.IsDisabled = isDisabled;
            this.IsExpired = isExpired;

            if (isExpired)
            {
                this.PasswordExpiryDate = DateTime.Now;
            }
        }

        public UserModel(string username, string email, string password, bool isDisabled, bool isExpired, DateTime passwordExpiryDate) : this(username, email, password, isDisabled, isExpired)
        {
            this.PasswordExpiryDate = passwordExpiryDate;
        }

        public UserModel(string username, string email, string password, bool isDisabled, bool isExpired, DateTime passwordExpiryDate, List<string> roles) : this(username, email, password, isDisabled, isExpired, passwordExpiryDate)
        {
            this.Roles = roles;
        }
    }
}
