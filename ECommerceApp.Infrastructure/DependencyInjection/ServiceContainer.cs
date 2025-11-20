using System.Text;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Application.Services.Interfaces.Logging;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Identity;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Domain.Interfaces.Authentication;
using ECommerceApp.Domain.Interfaces.Cart;
using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Infrastructure.Middleware;
using ECommerceApp.Infrastructure.Repositories;
using ECommerceApp.Infrastructure.Repositories.Authentication;
using ECommerceApp.Infrastructure.Repositories.Cart;
using ECommerceApp.Infrastructure.Services;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceApp.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration config)
    {
        string connectionString = config.GetConnectionString("DefaultConnection")!;
        
        services.AddDbContext<AppDbContext>(option =>
            option.UseNpgsql(connectionString).UseExceptionProcessor(),
            ServiceLifetime.Scoped
        );
        
        services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
        services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
        services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));

        services.AddDefaultIdentity<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredUniqueChars = 1;
        }).AddRoles<IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });

        services.AddScoped<IRoleManagement, RoleManagement>();
        services.AddScoped<IUserManagement, UserManagement>();
        services.AddScoped<ITokenManagement, TokenManagement>();
        services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
        services.AddScoped<IPaymentService, StripePaymentService>();
        
        return services;
    }

    public static IApplicationBuilder UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}