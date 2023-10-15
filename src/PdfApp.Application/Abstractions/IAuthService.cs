using PdfApp.Domain.Entities;

namespace PdfApp.Application.Abstractions;

public interface IAuthService
{
    string GenerateUserToken(User user);
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hashedPassword);
}
