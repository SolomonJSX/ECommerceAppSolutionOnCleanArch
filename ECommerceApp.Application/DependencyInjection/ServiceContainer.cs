using ECommerceApp.Application.Mapping;
using ECommerceApp.Application.Services.Implementations;
using ECommerceApp.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApp.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingConfig));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}