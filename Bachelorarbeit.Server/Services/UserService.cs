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
    public async Task<RegistrationResponse> AddUser(string email, string password, string username)
    {
        var userExists = await _userRepository.GetByEmailAsync(email);
        if (userExists != null)
        {
            return new RegistrationResponse()
            {
                Message = "user already exists",
                Success = false
            };
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
            CreatedAt = DateTime.Now
        };
        
        _userRepository.CreateAsync(user);
        _userRepository.SaveChangesAsync();
        
        return new RegistrationResponse()
        {
            Message = "user created",
            Success = true,
            Token = Convert.ToBase64String(refreshToken)
        };
    }
    public bool VerifyEmailAndPassword(string email, string password)
    {
        return true;
    }
    public byte[] GetRefreshToken(string email)
    {
        return new byte[0];
    }
    public RefreshTokenDto ReissueRefreshToken(byte[] oldRefreshToken)
    {
        return new RefreshTokenDto();
    }
    public string FindUsernameByToken(byte[] refreshToken)
    {
        return string.Empty;
    }

    public UserModel FindUserByUsername(string username)
    {
        return new UserModel();
    }
    
    public Task<bool> UpdateUser(UserModel entity)
    {
        return Task.FromResult(true);
    }
    public Task<bool>  ResetPasswordAsync(string email, string password)
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
        var salt = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;

    }
}