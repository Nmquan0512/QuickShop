using QuickShop.API.Models;
using QuickShop.API.Repositories;

namespace QuickShop.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly INotificationService _notificationService;

        public OrderService(IOrderRepository orderRepository, IProductService productService, INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _notificationService = notificationService;
        }

        public async Task<Order> CreateOrderAsync(OrderDto dto)
        {
            if (!await _productService.ProductExistsAsync(dto.ProductId))
                throw new ArgumentException("Product does not exist.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Status = "Pending"
            };

            await _orderRepository.AddAsync(order);
            await _notificationService.SendOrderConfirmationAsync(order);
            return order;
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null || order.Status != "Pending")
                return false;

            order.Status = "Cancelled";
            await _orderRepository.UpdateAsync(order);
            return true;
        }
    }
}
