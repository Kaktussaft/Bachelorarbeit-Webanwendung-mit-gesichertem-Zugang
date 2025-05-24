using Bachelorarbeit.Server.Controllers.Requests;
using Bachelorarbeit.Server.Controllers.Responses;
using Bachelorarbeit.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bachelorarbeit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IUserService _userService;

    public RegistrationController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Register([FromBody] RegistrationRequest request)
    {
        var result =   _userService.AddUser(request.userName, request.userMail, request.password).Result;

        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }
}