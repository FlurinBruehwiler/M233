using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Projektarbeit.Configurations;
using Projektarbeit.Models;
using static System.Text.Encoding;

namespace Projektarbeit.Services;

public class AuthService
{
        private readonly IOptions<AuthenticationConfiguration> _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IOptions<AuthenticationConfiguration> configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Email)
        };

        if (user.IsAdministrator)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
        }

        var key = new SymmetricSecurityKey(UTF8.GetBytes(_configuration.Value.Secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(100),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwt;
    }

    public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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

    public void AppendAccessToken(User user)
    {
        var httpResponse = _httpContextAccessor.HttpContext?.Response;

        if (httpResponse is null)
            throw new Exception("HttpResponse was null");

        httpResponse.Headers.Authorization = CreateToken(user);
        httpResponse.Headers.AccessControlAllowCredentials = "true";
    }
    
    public static void AdministratorRole(AuthorizationPolicyBuilder builder)
    {
        builder.RequireRole("Administrator");
    }
}