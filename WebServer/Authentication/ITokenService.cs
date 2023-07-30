using WebServer.Models;

namespace WebServer.Authentication
{
    public interface ITokenService<T>
    {
        T Authenticate(Login login);
        bool Validate(string token);
    }
}
