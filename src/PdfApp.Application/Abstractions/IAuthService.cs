using PdfApp.Domain.Entities;

namespace PdfApp.Application.Abstractions;

public interface IAuthService
{
    string GenerateUserToken(User user);
}
