using PdfApp.Application.Abstractions;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;

namespace PdfApp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> CreateAsync(Guid id, string userName, string email)
    {
        return _userRepository.CreateAsync(id, userName, email);
    }

    public Task<User?> GetByIdAsync(Guid id)
    {
        return _userRepository.GetByIdAsync(id);
    }
}
