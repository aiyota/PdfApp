using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using PdfApp.Application.Abstractions;
using PdfApp.Application.Services;

namespace PdfApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPdfService, PdfService>();
        return services;
    }
}