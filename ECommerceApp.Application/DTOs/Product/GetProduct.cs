using System.ComponentModel.DataAnnotations;
using ECommerceApp.Application.DTOs.Category;

namespace ECommerceApp.Application.DTOs.Product;

public class GetProduct : ProductBase
{
    [Required]
    public Guid Id { get; set; }
    public GetCategory? Category { get; set; }
}