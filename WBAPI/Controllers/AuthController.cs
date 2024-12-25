using Microsoft.AspNetCore.Mvc;
using WBAPI.Configuration;
using WBAPI.Models;
using WBAPI.Services;

namespace WBAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        _userService.Register(user);
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var (loggedInUser, token) = _userService.Login(user.Username, user.PasswordHash);
        if (loggedInUser == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}
