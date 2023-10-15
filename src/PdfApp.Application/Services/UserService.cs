using PdfApp.Application.Abstractions;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Exceptions;
using PdfApp.Domain.Exceptionsp;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace PdfApp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
                    ?? throw new UserNotFoundException(null, id);
        return user;
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _userRepository.GetByEmailAsync(email)
                    ?? throw new UserNotFoundException(email, null);
        return user;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
                    ?? throw new UserNotFoundException(email, null);
        return VerifyPassword(password, user.PasswordHash);
    }

    public async Task<User> RegisterAsync(
        string firstName,
        string lastName,
        string email,
        string password)
    {
        if (await _userRepository.GetByEmailAsync(email) is not null)
        {
            throw new DuplicateEmailException();
        }

        return await _userRepository.CreateUserAsync(
            firstName,
            lastName,
            email,
            HashPassword(password));
    }

    public async Task<User> UpdateUserAsync(
        Guid userId,
        string? email,
        string? firstName,
        string? lastName,
        string? password)
    {
        if (await _userRepository.GetByIdAsync(userId) is null)
        {
            throw new UserNotFoundException(null, userId);
        }

        var passwordHash = string.IsNullOrEmpty(password)
                            ? null
                            : HashPassword(password);
        return await _userRepository.UpdateAsync(
            userId,
            firstName,
            lastName,
            email,
            passwordHash);
    }

    public static string HashPassword(string password)
    {
        string salt = BC.GenerateSalt();
        string hashedPassword = BC.HashPassword(password, salt);
        return hashedPassword;
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BC.Verify(password, hashedPassword);
    }
}
