using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.DTOs.Category;

public class CategoryBase
{
    [Required]
    public string? Name { get; set; }
}