using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Punchclock.Models;
using static System.Text.Encoding;

namespace Punchclock.Services;

public class AuthService
{
    private readonly IOptions<AuthenticationConfiguration> _configuration;

    public AuthService(IOptions<AuthenticationConfiguration> configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Name)
        };

        var key = new SymmetricSecurityKey(UTF8.GetBytes(_configuration.Value.Secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(100),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwt;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    public void AppendAccessToken(HttpResponse httpResponse, User user)
    {
        httpResponse.Cookies.Append("dasToken", CreateToken(user), new CookieOptions
        {
            SameSite = SameSiteMode.Strict,
            Secure = true,
            Domain = null,
            HttpOnly = true,
            IsEssential = true
        });
        httpResponse.Headers.AccessControlAllowCredentials = "true";
    }
}