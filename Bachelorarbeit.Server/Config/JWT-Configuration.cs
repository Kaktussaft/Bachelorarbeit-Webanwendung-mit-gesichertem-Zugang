namespace Bachelorarbeit.Server.Config;

public class JWT_Configuration
{
    public string SecurityKey { get; set; }
    public string Audience { get; set; }
    public string Consumer { get; set; }
    public int ExpirationInMinutes { get; set; }
    
}