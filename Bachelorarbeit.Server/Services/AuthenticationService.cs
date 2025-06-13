using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bachelorarbeit.Server.Config;
using Bachelorarbeit.Server.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Bachelorarbeit.Server.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly JwtConfiguration _jwtConfiguration;

    public AuthenticationService(IUserService userService,
        JwtConfiguration jwtConfiguration)
    {
        _userService = userService;
        _jwtConfiguration = jwtConfiguration;
    }

    public AuthenticationResult Login(string email, string password)
    {
        var isValidUser = _userService.VerifyEmailAndPassword(email, password);

        if (!isValidUser)
        {
            return AuthenticationResult.FailedAuthenticationResult("user is not valid");
        }
        
        var username = _userService.FindUserByUserMail(email).Username;
        var accessToken = GenerateJwtToken(email, username);
        var refreshToken = Convert.ToBase64String(_userService.GetRefreshToken(email).Result);

        return AuthenticationResult.SuccessfulAuthenticationResult(
            new JwtSecurityTokenHandler().WriteToken(accessToken), accessToken.ValidTo, refreshToken);
    }

    public AuthenticationResult ReissueAccessToken(byte[] oldAccessToken)
    {
        var refreshTokenDto = _userService.ReissueRefreshToken(oldAccessToken);
        if (refreshTokenDto == null)
        {
            return AuthenticationResult.FailedAuthenticationResult("refresh token is not valid");
        }

        var accessToken = GenerateJwtToken(refreshTokenDto.Email, refreshTokenDto.Email);
        return AuthenticationResult.SuccessfulAuthenticationResult(
            new JwtSecurityTokenHandler().WriteToken(accessToken), accessToken.ValidTo,
            Convert.ToBase64String(refreshTokenDto.RefreshToken));
    }

    private JwtSecurityToken GenerateJwtToken(string email, string username)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var keyBytes = Encoding.UTF8.GetBytes(_jwtConfiguration.SecurityKey);
        
        if (keyBytes.Length < 32)
        {
            var paddedKey = new byte[32];
            Array.Copy(keyBytes, paddedKey, Math.Min(keyBytes.Length, 32));
            keyBytes = paddedKey;
        }
        
        var key = new SymmetricSecurityKey(keyBytes);
        var hashedKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var jasonWebToken = new JwtSecurityToken(_jwtConfiguration.Audience, _jwtConfiguration.Consumer, claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationInMinutes), signingCredentials: hashedKey);

        return jasonWebToken;
    }
}

public class AuthenticationResult
{
    public string? AccessToken { get; init; }
    public DateTime AccessTokenExpiration { get; init; }
    public string? RefreshToken { get; init; }
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }

    public static AuthenticationResult FailedAuthenticationResult(string errorMessage)
    {
        return new AuthenticationResult()
        {
            IsSuccess = false,
            Error = errorMessage
        };
    }

    public static AuthenticationResult SuccessfulAuthenticationResult(string accessToken,
        DateTime accessTokenExpiration, string refreshToken)
    {
        return new AuthenticationResult()
        {
            AccessToken = accessToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = refreshToken,
            IsSuccess = true
        };
    }
}