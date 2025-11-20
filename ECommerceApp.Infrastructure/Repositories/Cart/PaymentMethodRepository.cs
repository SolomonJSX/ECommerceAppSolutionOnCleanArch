using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interfaces.Cart;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories.Cart;

public class PaymentMethodRepository(AppDbContext context) : IPaymentMethod
{
    public async Task<IEnumerable<PaymentMethod>> GetPaymentMethodsAsync()
    {
        return await context.PaymentMethods.AsNoTracking().ToListAsync();
    }
}