using Microsoft.AspNetCore.Mvc;
using QuickShop.API.Models;
using QuickShop.API.Services;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(dto);
            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        var result = await _orderService.CancelOrderAsync(id);
        return result ? Ok() : BadRequest("Cannot cancel this order.");
    }
}