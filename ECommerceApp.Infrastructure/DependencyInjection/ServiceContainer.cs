using ECommerceApp.Application.Services.Interfaces.Logging;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Infrastructure.Middleware;
using ECommerceApp.Infrastructure.Repositories;
using ECommerceApp.Infrastructure.Services;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApp.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;
        
        services.AddDbContext<AppDbContext>(option =>
            option.UseNpgsql(connectionString).UseExceptionProcessor(),
            ServiceLifetime.Scoped
        );
        
        services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
        services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
        services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}