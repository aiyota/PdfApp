﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfApp.Application.Config;
using PdfApp.Domain.Abstractions;
using PdfApp.Infrastructure.Persistence;
using PdfApp.Infrastructure.Persistence.Repositories;

namespace PdfApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPdfRepository, PdfRepository>();

        return services;
    }
}
