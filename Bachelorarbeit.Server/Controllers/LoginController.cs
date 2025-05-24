using System;
using Bachelorarbeit.Server.Controllers.Requests;
using Bachelorarbeit.Server.Controllers.Responses;
using Microsoft.AspNetCore.Mvc;
using Bachelorarbeit.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace Bachelorarbeit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var loginResult = _authenticationService.Login(request.Email, request.Password);
        if (!loginResult.IsSuccess)
            return BadRequest(LoginResponse.GetFailedLoginResponse());

        return Ok(LoginResponse.GetSuccessfullLoginResponse(loginResult.RefreshToken));
    }
}