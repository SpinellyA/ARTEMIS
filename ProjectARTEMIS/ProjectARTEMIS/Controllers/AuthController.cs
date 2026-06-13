
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[AllowAnonymous]
[Route("/api/v1/auth/")]
public class AuthController : ControllerBase
{
    private readonly UserService _user;

    public AuthController(UserService user)
    {
        _user = user;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var token = _user.LoginAsync(req);

        return Ok(token);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var token = _user.RegisterAsync(req);
        return Ok(token);
    }
}