using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Cart;

namespace ECommerceApp.Application.Services.Interfaces.Cart;

public interface ICartService
{
    Task<ServiceResponse> SaveCheckoutHistories(IEnumerable<CreateAchieve> checkouts);
    Task<ServiceResponse> Checkout(Checkout checkout, string userId);
}