using Microsoft.AspNetCore.Mvc;
using Bachelorarbeit.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Bachelorarbeit.Server.Controllers.Requests;
using Bachelorarbeit.Server.Dtos;


namespace Bachelorarbeit.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class RefreshTokenController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public RefreshTokenController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }


    [AllowAnonymous]
    [HttpPost]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest argRequest)
    {
        if(argRequest?.RefreshToken == null)
            return BadRequest("Invalid request");

        var oldToken = Convert.FromBase64String(argRequest.RefreshToken);
        var newToken = _authenticationService.ReissueAccessToken(oldToken);
        
        if(!newToken.IsSuccess)
            return BadRequest("Invalid refresh token");
        
        return Ok(new LoginTokenDto
        {
            AccessToken = newToken.AccessToken,
            Expiration = newToken.AccessTokenExpiration,
            RefreshToken = newToken.RefreshToken
        });
    }
}