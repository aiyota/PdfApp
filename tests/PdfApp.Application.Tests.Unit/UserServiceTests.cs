using Microsoft.Extensions.Options;
using NSubstitute;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Config;
using PdfApp.Application.Services;
using PdfApp.Domain.Abstractions;
using PdfApp.Domain.Entities;
using PdfApp.Domain.Exceptions;

namespace PdfApp.Application.Tests.Unit;

public class UserServiceTests
{
    private readonly IUserService _sut;
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    public UserServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "3078ebd17f394cda8ea4e6b95a74bf34",
            ExpiryMinutes = 1,
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        };
        _sut = new UserService(
            _userRepository,
            new AuthService(Options.Create(jwtSettings)));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(null as User);
        await Assert.ThrowsAsync<UserNotFoundException>(() => _sut.GetByIdAsync(Guid.Empty));
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        _userRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(null as User);
        await Assert.ThrowsAsync<UserNotFoundException>(() => _sut.GetByEmailAsync("fakeemail@test.com"));
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowDuplicateEmailException_WhenUserExists()
    {
        _userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(new User());
        await Assert.ThrowsAsync<DuplicateEmailException>(() =>
            _sut.RegisterAsync(
            "Test",
            "User",
            "User@test.com",
            "test password"));
    }
}
