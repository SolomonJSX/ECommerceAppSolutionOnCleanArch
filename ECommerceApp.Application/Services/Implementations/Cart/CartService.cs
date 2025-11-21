using AutoMapper;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interfaces.Cart;

namespace ECommerceApp.Application.Services.Implementations.Cart;

public class CartServic(ICart cartEntity, IMapper mapper) : ICartService
{
    public async Task<ServiceResponse> SaveCheckoutHistories(IEnumerable<CreateAchieve> checkouts)
    {
        var mappedData = mapper.Map<IEnumerable<Achieve>>(checkouts);
        
        var result = await cartEntity.SaveCheckoutHistory(mappedData);
        
        return result > 0 ? new ServiceResponse(true, "Checkout Achieved") : new ServiceResponse(false, "Checkout Failed");
    }

    public Task<ServiceResponse> Checkout(Checkout checkout, string userId)
    {
        throw new NotImplementedException();
    }
}