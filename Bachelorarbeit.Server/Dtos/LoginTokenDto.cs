namespace Bachelorarbeit.Server.Dtos;

public class LoginTokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; } 
}