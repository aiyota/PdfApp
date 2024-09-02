using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Config;
using PdfApp.Presentation.Api.Abstractions;
using PdfApp.Presentation.Api.Contracts;

namespace PdfApp.Presentation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ApiControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtSettings _jwtSettings;

    public UserController(
        IAuthService authService,
        JwtSettings jwtSettings)
    {
        _authService = authService;
        _jwtSettings = jwtSettings;
    }

    [AllowAnonymous]
    [HttpGet(ApiRoutes.User.Login)]
    public IActionResult Login(string token)
    {
        var user = _authService.GetUserFromToken(token);

        // If no user can be found, it isn't a valid token
        if (user is null)
        {
            return Unauthorized();
        }

        SetTokenAsCookie(token, _jwtSettings.ExpiryMinutes);

        return Ok(new { user });
    }

    [AllowAnonymous]
    [HttpPost(ApiRoutes.User.Logout)]
    public IActionResult Logout()
    {
        RemoveTokenFromCookies();
        return Ok();
    }

    [HttpGet(ApiRoutes.User.GetCurrentUser)]
    public IActionResult GetCurrentUser()
    {
        var user = _authService.GetUserFromClaimsPrinciple(User);
        if (user is null)
        {
            return Unauthorized();
        }

        return Ok(new { user });
    }
}
