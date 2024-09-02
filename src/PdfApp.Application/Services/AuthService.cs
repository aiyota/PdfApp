using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Config;
using PdfApp.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace PdfApp.Application.Services;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;

    public AuthService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }


    public string GenerateUserToken(User user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };


        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public ClaimsUser? GetUserFromToken(string token)
    {
        var claimsPrincipal = GetPrinciplesFromToken(token);
        return GetUserFromClaimsPrinciple(claimsPrincipal);
    }

    public ClaimsUser? GetUserFromClaimsPrinciple(ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var userName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
        if (id is null || email is null || userName is null)
            return null;

        return new ClaimsUser
        {
            Id = Guid.Parse(id),
            Email = email,
            UserName = userName
        };
    }

    private ClaimsPrincipal GetPrinciplesFromToken(string token)
    {
        var validation = new TokenValidationParameters
        {
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)),
            ValidateLifetime = false
        };

        var claimsPrinciple = new JwtSecurityTokenHandler().ValidateToken(token, validation, out _)
            ?? throw new SecurityTokenException("Invalid token");

        return claimsPrinciple;
    }

    public string HashPassword(string password)
    {
        string salt = BC.GenerateSalt();
        string hashedPassword = BC.HashPassword(password, salt);
        return hashedPassword;
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}
