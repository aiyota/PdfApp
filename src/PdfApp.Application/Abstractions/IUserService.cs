using PdfApp.Domain.Entities;

namespace PdfApp.Application.Abstractions;

public interface IUserService
{
    Task<User> RegisterAsync(
        string firstName,
        string lastName,
        string email,
        string password);

    Task<bool> LoginAsync(
        string email,
        string password);

    Task<User?> GetByIdAsync(Guid id);

    Task<User?> GetByEmailAsync(string email);

    Task<User> UpdateUserAsync(
        Guid userId,
        string? email,
        string? firstName,
        string? lastName,
        string? password);
}