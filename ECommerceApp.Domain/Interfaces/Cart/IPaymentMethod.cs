using ECommerceApp.Domain.Entities.Cart;

namespace ECommerceApp.Domain.Interfaces.Cart;

public interface IPaymentMethod
{
    Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync();
}