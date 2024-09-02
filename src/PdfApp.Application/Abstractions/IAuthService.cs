using PdfApp.Domain.Entities;
using System.Security.Claims;

namespace PdfApp.Application.Abstractions;

public interface IAuthService
{
    string GenerateUserToken(User user);
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashedPassword);
    ClaimsUser? GetUserFromToken(string token);
    ClaimsUser? GetUserFromClaimsPrinciple(ClaimsPrincipal claimsPrincipal);
}
