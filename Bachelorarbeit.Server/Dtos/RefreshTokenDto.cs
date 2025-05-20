namespace Bachelorarbeit.Server.Dtos;

public class RefreshTokenDto
{
    public byte[] RefreshToken { get; set; }
    public string Email { get; set; }
}