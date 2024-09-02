using PdfApp.Domain.Entities;

namespace PdfApp.Application.Abstractions;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(Guid id, string userName, string email);
}
