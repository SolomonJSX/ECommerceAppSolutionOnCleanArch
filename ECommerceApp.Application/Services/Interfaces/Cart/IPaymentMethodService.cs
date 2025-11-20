using ECommerceApp.Application.DTOs.Cart;

namespace ECommerceApp.Application.Services.Interfaces.Cart;

public interface IPaymentMethodService
{
    Task<IEnumerable<GetPaymentMethod>> GetPaymentMethods();
}