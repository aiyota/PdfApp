using FluentAssertions;
using Microsoft.Extensions.Options;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Config;
using PdfApp.Application.Services;
using PdfApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            PasswordHash = "TestPasswordHash",
        };

        var token = _sut.GenerateUserToken(user);
        token.Should().NotBeNull();
    }
}
