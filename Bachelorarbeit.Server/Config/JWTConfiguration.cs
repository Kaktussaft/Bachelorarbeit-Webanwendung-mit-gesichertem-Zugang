

namespace Bachelorarbeit.Server.Config;

public class JwtConfiguration
{
    
    public required string SecurityKey { get; set; }
    public required string Audience { get; set; }
    public required string Consumer { get; set; }
    public int ExpirationInMinutes { get; set; }
    
}