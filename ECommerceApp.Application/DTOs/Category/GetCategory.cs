using ECommerceApp.Application.DTOs.Product;

namespace ECommerceApp.Application.DTOs.Category;

public class GetCategory : CategoryBase
{
    public Guid Id { get; set; }
    public ICollection<GetProduct>? Products { get; set; }
}