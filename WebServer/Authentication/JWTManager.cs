using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebServer.Models;
using Microsoft.EntityFrameworkCore;
using WebServer.Services.Contexts;
using WebServer.Models.UserData;
using System.IdentityModel.Tokens.Jwt;
using WebServer.Controllers;

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

        public async Task<JWTToken> Authenticate(Login login)
        {
            User user = await userContext.Users.SingleOrDefaultAsync(u => u.Username.Equals(login.Username) && u.Password.Equals(login.Password));

            if (user.PasswordExpiryDate < DateTime.Now) 
            {
                user.IsExpired = true;
                userContext.Users.Update(user);
                await userContext.SaveChangesAsync();

                return new JWTToken { Token = "invalid_token", IsAuthSuccessful = false, ErrorMessage = "Password expired. Contact administrator." };
            }

            if (user == null || user == default)
            {
                return new JWTToken { Token = "invalid_token", IsAuthSuccessful = false, ErrorMessage = "Invalid credentials." };
            }
            else
            {
                if (user.IsDisabled)
                {
                    return new JWTToken { Token = "invalid_token", IsAuthSuccessful = false, ErrorMessage = "Account disabled." };
                }
                else if (user.IsExpired)
                {
                    return new JWTToken { Token = "invalid_token", IsAuthSuccessful = false, ErrorMessage = "Password expired. Contact administrator." };
                }
                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new(ClaimTypes.Name, user.Username),
                        new(ClaimTypes.NameIdentifier, user.FirstName + " " + (user.MiddleName != null ? user.MiddleName.First() + ". " : "") + user.LastName),
                        new(ClaimTypes.Role, user.Role),
                        new(JwtRegisteredClaimNames.Jti, user.UUID.ToString()),
                        new(JwtRegisteredClaimNames.Aud, configuration["Jwt:Audience"]),
                        new(JwtRegisteredClaimNames.Iss, configuration["Jwt:Issuer"])
                        }),
                        Expires = DateTime.UtcNow.AddMinutes(60 * 8),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    JWTToken JWTToken = new JWTToken { Token = tokenHandler.WriteToken(token), IsAuthSuccessful = true };

                    return JWTToken;
                }
            }
        }

        public async Task<bool> Validate(string token)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var parsed = JwtParser.ParseClaimsFromJwt(token);


            if (!parsed.IsNullOrEmpty())
            {
                var UUID = parsed.Single(c => c.Type.Equals("jti")).Value;
                var user = await userContext.Users.SingleOrDefaultAsync(u => u.UUID.Equals(Guid.Parse(UUID)));

                if (user != null)
                {
                    if (user.IsDisabled || user.IsExpired)
                    {
                        return false;
                    }
                }
            }

            try
            {
                if (token != "invalid_token")
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
