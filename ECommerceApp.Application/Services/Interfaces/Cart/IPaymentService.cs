using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.DTOs.Cart;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Services.Interfaces.Cart;

public interface IPaymentService
{
    Task<ServiceResponse> Pay(decimal amount, IEnumerable<Product> cartProducts, IEnumerable<ProcessCart> carts);
}