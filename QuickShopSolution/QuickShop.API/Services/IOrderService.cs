using QuickShop.API.Models;

namespace QuickShop.API.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(OrderDto dto);
        Task<bool> CancelOrderAsync(Guid orderId);
    }
}
