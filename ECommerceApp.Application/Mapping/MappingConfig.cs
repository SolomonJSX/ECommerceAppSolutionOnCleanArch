using AutoMapper;
using ECommerceApp.Application.DTOs.Cart;
using ECommerceApp.Application.DTOs.Category;
using ECommerceApp.Application.DTOs.Identity;
using ECommerceApp.Application.DTOs.Product;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Entities.Identity;

namespace ECommerceApp.Application.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateCategory, Category>();
        CreateMap<CreateProduct, Product>();

        CreateMap<Category, GetCategory>();
        CreateMap<Product, GetProduct>();
        
        CreateMap<CreateUser, AppUser>();
        CreateMap<LoginUser, AppUser>();
        
        CreateMap<PaymentMethod, GetPaymentMethod>();
        CreateMap<CreateAchieve, Achieve>();
    }
}