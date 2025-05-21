using Bachelorarbeit.Server.Config;
using Bachelorarbeit.Server.Interfaces;

namespace Bachelorarbeit.Server.Services;

public class AuthenticationService 
{
    private readonly IUserService _userService;
    private readonly JWT_Configuration _jwtConfiguration;
    public AuthenticationService(IUserService userService, JWT_Configuration jwtConfiguration)
    {
        _userService = userService;
        _jwtConfiguration = jwtConfiguration;
    }
    
    
    
}