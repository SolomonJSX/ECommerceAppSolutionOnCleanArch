using ECommerceApp.Application.Mapping;
using ECommerceApp.Application.Services.Implementations;
using ECommerceApp.Application.Services.Implementations.Authentication;
using ECommerceApp.Application.Services.Interfaces;
using ECommerceApp.Application.Services.Interfaces.Authentication;
using ECommerceApp.Application.Validations;
using ECommerceApp.Application.Validations.Authentication;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApp.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingConfig));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

        services.AddScoped<IValidationService, ValidationService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        
        return services;
    }
}