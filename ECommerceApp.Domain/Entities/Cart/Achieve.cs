using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities.Cart;

public class Achieve
{
    [Key]
    public Guid Id { get; set; } =  Guid.NewGuid();
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}