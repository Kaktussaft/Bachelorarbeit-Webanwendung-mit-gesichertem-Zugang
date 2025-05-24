using Bachelorarbeit.Server.Dtos;
using Bachelorarbeit.Server.Models;
using Bachelorarbeit.Server.Controllers.Responses;

namespace Bachelorarbeit.Server.Interfaces;

public interface IUserService
{
    Task<RegistrationResponse> AddUser(string email, string password, string username);
    bool VerifyEmailAndPassword( string email, string password);
    Task<byte[]> GetRefreshToken(string email);
    RefreshTokenDto ReissueRefreshToken(byte[] oldRefreshToken);
    UserModel FindUserByUsername(string username);
    Task<bool> UpdateUser(UserModel entity);
    Task<bool> ResetPasswordAsync(string email, string password);
    byte[] GeneratePasswordSalt();
}