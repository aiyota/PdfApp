using PdfApp.Application.Abstractions;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Exceptions;

namespace PdfApp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public UserService(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id)
                    ?? throw new UserNotFoundException(null, id);
        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email)
                    ?? throw new UserNotFoundException(email, null);
        return user;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email)
                    ?? throw new UserNotFoundException(email, null);
        return _authService.VerifyPassword(password, user.PasswordHash);
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
            _authService.HashPassword(password));
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
                            : _authService.HashPassword(password);
        return await _userRepository.UpdateAsync(
            userId,
            firstName,
            lastName,
            email,
            passwordHash);
    }
}
