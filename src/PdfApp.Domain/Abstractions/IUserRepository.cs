using PdfApp.Domain.Entities;

namespace PdfApp.Domain.Abstractions;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(Guid id, string userName, string email);
}
