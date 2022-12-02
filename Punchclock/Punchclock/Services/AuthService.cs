using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Punchclock.Configurations;
using Punchclock.Models.Db;
using static System.Text.Encoding;

namespace Punchclock.Services;

public class AuthService
{
    private readonly IOptions<AuthenticationConfiguration> _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IOptions<AuthenticationConfiguration> configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string CreateToken(ApplicationUser applicationUser)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, applicationUser.Name)
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

    public void AppendAccessToken(ApplicationUser applicationUser)
    {
        var httpResponse = _httpContextAccessor.HttpContext?.Response;

        if (httpResponse is null)
            throw new Exception("HttpResponse was null");
        
        httpResponse.Cookies.Append("dasToken", CreateToken(applicationUser), new CookieOptions
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