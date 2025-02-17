using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using Database;
using Domain.Models;
using Services.Interfaces;

public class AuthService : IAuthService
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _config;

    public AuthService(DatabaseContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    /** Register a new user */
    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        if (await _context.Users.AnyAsync(u => u.EmailAddress == email))
        {
            return false; // User already exists
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User { EmailAddress = email, PasswordHash = hashedPassword };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    /** Validate user credentials and return JWT token */
    public async Task<string> AuthenticateUserAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return null; // Invalid credentials
        }

        return GenerateJwtToken(user);
    }

    /** Generate JWT Token */
    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.EmailAddress)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
