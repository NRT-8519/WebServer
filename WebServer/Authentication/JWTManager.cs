using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebServer.Models;
using Microsoft.EntityFrameworkCore;
using WebServer.Services.Contexts;
using WebServer.Models.UserData;

namespace WebServer.Authentication
{
    public class JWTManager : ITokenService<JWTToken>
    {
        private readonly UserContext userContext;
        private readonly IConfiguration configuration;

        public JWTManager(UserContext userContext, IConfiguration configuration)
        {
            this.userContext = userContext;
            this.configuration = configuration;
        }

        public JWTToken Authenticate(Login login)
        {
            User user = userContext.Users.Include(u => u.Emails).Include(u => u.PhoneNumbers).Include(u => u.Roles).FirstOrDefault(u => u.Username.Equals(login.Username) && u.Password.Equals(login.Password));

            if (user == null || user == default)
            {
                return null;
            }
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Roles.First().Role),
                    new Claim(JwtRegisteredClaimNames.Jti, user.UUID.ToString()),
                    new Claim(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"])
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new JWTToken { Token = tokenHandler.WriteToken(token) };
            }
        }
    }
}
