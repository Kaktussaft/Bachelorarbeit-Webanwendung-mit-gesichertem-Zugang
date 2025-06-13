using System.Security.Cryptography;
using Bachelorarbeit.Server.Dtos;
using Bachelorarbeit.Server.Interfaces;
using Bachelorarbeit.Server.Models;
using Bachelorarbeit.Server.Controllers.Requests;
using Bachelorarbeit.Server.Controllers.Responses;

namespace Bachelorarbeit.Server.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegistrationResponse> AddUser(string username, string email, string password )
    {
        var userMailExists = await _userRepository.GetByEmailAsync(email);
        var userUsernameExists = await _userRepository.GetByUsernameAsync(username);

        if (userUsernameExists != null)
        {
            return RegistrationResponse.GetUserNameExistsResponse();
        }

        if (userMailExists != null)
        {
            return RegistrationResponse.GetUserMailExistsResponse();
        }

        var salt = GeneratePasswordSalt();
        var hash = GeneratePasswordHash(password, salt);
        var refreshToken = GenerateRefreshToken();

        var user = new UserModel
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt,
            RefreshToken = refreshToken,
            Username = username,
            CreatedAt =  DateTimeOffset.Now.ToUniversalTime()

        };

        await _userRepository.CreateAsync(user);

        return RegistrationResponse.GetSuccessfullRegistrationResponse();
    }

    public bool VerifyEmailAndPassword(string email, string password)
    {
        var user = _userRepository.GetByEmailAsync(email).Result;
        var hash = GeneratePasswordHash(password, user.PasswordSalt);
        var correctPassword = hash.SequenceEqual(user.PasswordHash);
        return correctPassword;
    }

    public async Task<byte[]> GetRefreshToken(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user?.RefreshToken;
    }

    public RefreshTokenDto ReissueRefreshToken(byte[] oldRefreshToken)
    {
        var user = _userRepository.GetUserByRefreshTokenAsync(oldRefreshToken).Result;
        if (user == null)
            return null;
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        _userRepository.UpdateAsync(user).Wait();

        return new RefreshTokenDto
        {
            RefreshToken = newRefreshToken,
            Email = user.Email
        };
    }

    public UserModel FindUserByUsername(string username)
    {
        var user = _userRepository.GetByUsernameAsync(username).Result;
        return user;
    }

    public Task<bool> UpdateUser(UserModel entity)
    {
        return Task.FromResult(true);
    }

    public Task<bool> ResetPasswordAsync(string email, string password)
    {
        return Task.FromResult(true);
    }

    private byte[] GeneratePasswordHash(string password, byte[] salt)
    {
        using (var hmac = new HMACSHA512(salt))
        {
            return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    private byte[] GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return randomNumber;
        }
    }

    public byte[] GeneratePasswordSalt()
    {
        var salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
            return salt;
        }
    }
}