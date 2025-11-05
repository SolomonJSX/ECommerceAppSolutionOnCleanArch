using AutoMapper;
using ECommerceApp.Application.DTOs.Category;
using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateCategory, Category>();
        CreateMap<CreateProduct, Product>();

        CreateMap<Category, GetCategory>();
        CreateMap<Product, GetProduct>();
    }
}