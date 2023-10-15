using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Config;
using PdfApp.Application.Services;
using PdfApp.Presentation.Api.Abstractions;
using PdfApp.Presentation.Api.Contracts;
using PdfApp.Presentation.Api.Contracts.User;
using PdfApp.Presentation.Api.Mapping;

namespace PdfApp.Presentation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ApiControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly JwtSettings _jwtSettings;

    public UserController(
        IUserService userService,
        IAuthService authService,
        JwtSettings jwtSettings)
    {
        _userService = userService;
        _authService = authService;
        _jwtSettings = jwtSettings;
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Register)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var userRecord = await _userService.RegisterAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var user = userRecord.DomainToResponse();
        var token = _authService.GenerateUserToken(userRecord);
        SetTokenAsCookie(token, _jwtSettings.ExpiryMinutes);

        return Ok(new { user, token });
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Login)]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var loginSuccess = await _userService.LoginAsync(request.Email, request.Password);
        if (!loginSuccess)
        {
            return Unauthorized();
        }

        var userRecord = await _userService.GetByEmailAsync(request.Email);
        if (userRecord is null)
        {
            return NotFound();
        }

        var user = userRecord.DomainToResponse();
        var token = _authService.GenerateUserToken(userRecord);

        SetTokenAsCookie(token, _jwtSettings.ExpiryMinutes);

        return Ok(new { user, token });
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Logout)]
    public IActionResult Logout(HttpContext context)
    {
        RemoveTokenFromCookies();
        return Ok();
    }

    [HttpPost(ApiRoutes.User.UpdateCurrentUser)]
    public async Task<IActionResult> UpdateCurrentUser(UserUpdateRequest request)
    {
        if (UserId is null)
        {
            return Unauthorized();
        }

        var userDomain = await _userService.UpdateUserAsync(
            UserId.Value,
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);

        var user = userDomain.DomainToResponse();

        return Ok(new { user });
    }

    [HttpGet(ApiRoutes.User.GetCurrentUser)]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (UserId is null)
        {
            return Unauthorized();
        }

        var userDomain = await _userService.GetByIdAsync(UserId.Value);
        if (userDomain is null)
        {
            return NotFound();
        }

        var user = userDomain.DomainToResponse();

        return Ok(new { user });
    }
}
