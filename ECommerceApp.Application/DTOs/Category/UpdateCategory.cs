using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Application.DTOs.Category;

public class UpdateCategory : CategoryBase
{
    [Required]
    public Guid Id { get; set; }
}