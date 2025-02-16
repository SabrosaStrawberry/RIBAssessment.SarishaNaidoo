using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] LoginRequest request)
    {
        bool success = await _authService.RegisterUserAsync(request.Email, request.Password);
        if (!success)
        {
            return BadRequest(new { message = "User already exists" });
        }

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest request)
    {
        var token = await _authService.AuthenticateUserAsync(request.Email, request.Password);
        if (token == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
        var t = new TokenResponse { Token = token };
        return Ok(t);
    }
}
