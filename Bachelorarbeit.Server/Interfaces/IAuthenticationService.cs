using Bachelorarbeit.Server.Services;

namespace Bachelorarbeit.Server.Interfaces;

public interface IAuthenticationService
{
    AuthenticationResult Login(string email, string password);
    AuthenticationResult ReissueAccessToken(byte[] oldAccessToken);
}