﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using SachkovTech.Accounts.Infrastructure;
using SachkovTech.Files.Infrastructure;
using SachkovTech.Files.Presentation;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Infrastructure;
using SachkovTech.Issues.Presentation;
using Serilog.Events;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SachkovTech.Core.Options;
using SachkovTech.Accounts.Presentation;
using SachkovTech.Core.Abstractions;
using SachkovTech.Framework;
using SachkovTech.Framework.Models;
using SachkovTech.Accounts.Application;

namespace SachkovTech.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAccountsInfrastructure(configuration);
        services.AddAccountsApplication(configuration);
        services.AddAccountsPresentation();

        return services;
    }

    public static IServiceCollection AddFilesModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFilesInfrastructure(configuration);
        services.AddFilesPresentation();

        return services;
    }

    public static IServiceCollection AddIssuesModule(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIssuesInfrastructure(configuration);
        services.AddIssuesPresentation();

        return services;
    }

    public static IServiceCollection AddAuthServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing jwt configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });

        services.AddAuthorization();

        return services;
    }
    
    public static IServiceCollection AddLogging(
        this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(configuration.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

        services.AddSerilog();

        return services;
    }
    
    public static IServiceCollection AddFramework(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<UserScopedData>();
        return services;
    }

    public static IServiceCollection AddApplicationLayers(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            typeof(SachkovTech.Accounts.Application.DependencyInjection).Assembly,
            typeof(SachkovTech.Files.Application.DependencyInjection).Assembly,
            typeof(SachkovTech.Issues.Application.DependencyInjection).Assembly,
        };

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}