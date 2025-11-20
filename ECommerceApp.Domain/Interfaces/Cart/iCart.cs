using ECommerceApp.Domain.Entities.Cart;

namespace ECommerceApp.Domain.Interfaces.Cart;

public interface ICart
{
    Task<int> SaveCheckoutHistory(IEnumerable<Achieve> checkouts);
}