using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Domain.Interfaces.Cart;

namespace ECommerceApp.Application.Services.Implementations.Cart;

public class CartServic(ICart cartEntity, IMapper mapper, IGeneric<Product> productRepository) : ICartService
{
    public async Task<ServiceResponse> SaveCheckoutHistories(IEnumerable<CreateAchieve> checkouts)
    {
        var mappedData = mapper.Map<IEnumerable<Achieve>>(checkouts);
        
        var result = await cartEntity.SaveCheckoutHistory(mappedData);
        
        return result > 0 ? new ServiceResponse(true, "Checkout Achieved") : new ServiceResponse(false, "Checkout Failed");
    }

    public async Task<ServiceResponse> Checkout(Checkout checkout, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<(IEnumerable<Product>, decimal)> GetTotalAmount(IEnumerable<ProcessCart> processCarts)
    {
        if (!processCarts.Any()) return ([], 0);

        var products = await productRepository.GetAllAsync();

        if (!products.Any()) return ([], 0);

        var carts = processCarts
            .Select(cartItem => products.FirstOrDefault(p => p.Id == cartItem.ProductId))
            .Where(p => p != null)
            .ToList();

        var totalAmount = processCarts
            .Where(cartItem => products.Any(p => p.Id == cartItem.ProductId))
            .Sum(cartItem => cartItem.Quantity * carts.First(p => p.Id == cartItem.ProductId).Price);
        
        return (carts, totalAmount);
    } 
}