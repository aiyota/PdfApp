using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PdfApp.Application.Config;
using PdfApp.Presentation.Api.Common;
using System.Text;

namespace PdfApp.Presentation.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(
    this IServiceCollection services,
    ConfigurationManager configuration)
    {
        services.AddSingleton<ProblemDetailsFactory, PdfAppProblemDetailsFactory>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Authorization header using the Bearer token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        JwtSettings jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()
                                  ?? throw new NullReferenceException("JwtSettings is null");
        services.AddSingleton(jwtSettings);
        services
            .AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                            .RequireAuthenticatedUser()
                                            .AddAuthenticationSchemes("Bearer")
                                            .Build();
            });

        services
            .AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Events = AuthEventsHandler.Instance;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                };
            });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                string[] allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                                            ?? [];
                if (allowedOrigins.Length == 0)
                {
                    throw new Exception("AllowedOrigins is empty. Please set them in appSettings.json.");
                }

                builder.WithOrigins(
                        allowedOrigins)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
        });

        return services;
    }
}
