using ECommerceApp.Application.DTOs.Category;

namespace ECommerceApp.Application.DTOs.Product;

public class GetProduct : ProductBase
{
    public Guid Id { get; set; }
    public GetCategory? Category { get; set; }
}