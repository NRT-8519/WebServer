using WebServer.Models;

namespace WebServer.Authentication
{
    public interface ITokenService<T>
    {
        Task<T> Authenticate(Login login);
        Task<bool> Validate(string token);
    }
}
