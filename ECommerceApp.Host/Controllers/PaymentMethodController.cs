using ECommerceApp.Application.DTOs.Cart;
using ECommerceApp.Application.Services.Interfaces.Cart;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Host.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentMethodController(IPaymentMethodService paymentMethodService) : ControllerBase
{
    [HttpGet("payment-method")]
    public async Task<ActionResult<IEnumerable<GetPaymentMethod>>> GetPaymentMethod()
    {
        var methods = await paymentMethodService.GetPaymentMethods();

        if (!methods.Any())
            return NotFound();
        
        return Ok(methods);
    }
}