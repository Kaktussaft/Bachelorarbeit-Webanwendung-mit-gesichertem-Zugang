using System.ComponentModel.DataAnnotations;

namespace Bachelorarbeit.Server.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public byte[] RefreshToken { get; set; }
    public bool IsAdmin { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    
}