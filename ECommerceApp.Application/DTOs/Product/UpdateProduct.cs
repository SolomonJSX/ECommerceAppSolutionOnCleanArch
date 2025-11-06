using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.DTOs.Product;

public class UpdateProduct : ProductBase
{
    [Required]
    public Guid Id { get; set; }
}