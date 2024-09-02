using FluentAssertions;
using Microsoft.Extensions.Options;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Config;
using PdfApp.Application.Services;
using PdfApp.Domain.Entities;

namespace PdfApp.Application.Tests.Unit;

public class AuthServiceTests
{
    private readonly IAuthService _sut;

    public AuthServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "3078ebd17f394cda8ea4e6b95a74bf34",
            ExpiryMinutes = 1,
            Issuer = "TestIssuer",
            Audience = "TestAudience"
        };
        _sut = new AuthService(Options.Create(jwtSettings));
    }

    [Fact]
    public void GenerateUserToken_ReturnToken_WhenUserIsValid()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = "Test",
            Email = "test@test.com",
        };

        var token = _sut.GenerateUserToken(user);
        token.Should().NotBeNull();
    }

    [Fact]
    public void HashPassword_ReturnValidHashedPassword_WhenPasswordIsValid()
    {
        var password = "TestPassword";
        var hashedPassword = _sut.HashPassword(password);
        var isValidHashedPassword = _sut.VerifyPassword(password, hashedPassword);
        var isotValidHashedPassword = _sut.VerifyPassword("wrongPassword", hashedPassword);

        hashedPassword.Should().NotBeNull();
        isValidHashedPassword.Should().BeTrue();
        isotValidHashedPassword.Should().BeFalse();
    }
}
