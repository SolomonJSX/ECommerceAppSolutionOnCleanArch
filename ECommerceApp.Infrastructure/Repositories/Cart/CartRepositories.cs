using ECommerceApp.Domain.Entities.Cart;
using ECommerceApp.Domain.Interfaces.Cart;
using ECommerceApp.Infrastructure.Data;

namespace ECommerceApp.Infrastructure.Repositories.Cart;

public class CartRepositories(AppDbContext dbContext) : ICart
{
    public async Task<int> SaveCheckoutHistory(IEnumerable<Achieve> checkouts)
    {
        await dbContext.CheckoutAchieves.AddRangeAsync(checkouts);
        return await dbContext.SaveChangesAsync();
    }
}