using AutoMapper;
using ECommerceApp.Application.DTOs.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using ECommerceApp.Domain.Interfaces.Cart;

namespace ECommerceApp.Application.Services.Implementations.Cart;

public class PaymentMethodService(IPaymentMethod paymentMethod, IMapper mapper) : IPaymentMethodService
{
    public async Task<IEnumerable<GetPaymentMethod>> GetPaymentMethods()
    {
        var methods = await paymentMethod.GetPaymentMethodsAsync();

        if (!methods.Any()) return [];
        
        return mapper.Map<IEnumerable<GetPaymentMethod>>(methods);
    }
}