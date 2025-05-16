using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using QuickShop.API.Models;
using QuickShop.API.Repositories;
using QuickShop.API.Services;

namespace QuickShop.UnitTests
{
    public class KiemThuDichVuDonHang
    {
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly Mock<IProductService> _productService = new();
        private readonly Mock<INotificationService> _notification = new();

        private readonly OrderService _service;

        public KiemThuDichVuDonHang()
        {
            _service = new OrderService(
                _orderRepo.Object,
                _productService.Object,
                _notification.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldReturnOrder_WhenProductExists()
        {
            // Arrange
            var dto = new OrderDto { ProductId = "P1", Quantity = 2 };
            _productService.Setup(p => p.ProductExistsAsync("P1")).ReturnsAsync(true);

            // Act
            var result = await _service.CreateOrderAsync(dto);

            // Assert
            Assert.Equal("P1", result.ProductId);
            Assert.Equal(2, result.Quantity);
            Assert.Equal("Pending", result.Status);
            _orderRepo.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            _notification.Verify(n => n.SendOrderConfirmationAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrow_WhenProductNotExists()
        {
            var dto = new OrderDto { ProductId = "Invalid", Quantity = 1 };
            _productService.Setup(p => p.ProductExistsAsync("Invalid")).ReturnsAsync(false);

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldReturnTrue_WhenStatusIsPending()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Status = "Pending" };
            _orderRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            var result = await _service.CancelOrderAsync(orderId);

            Assert.True(result);
            Assert.Equal("Cancelled", order.Status);
            _orderRepo.Verify(r => r.UpdateAsync(order), Times.Once);
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldReturnFalse_WhenOrderNotFound()
        {
            var orderId = Guid.NewGuid();
            _orderRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync((Order)null);

            var result = await _service.CancelOrderAsync(orderId);

            Assert.False(result);
        }

        [Fact]
        public async Task CancelOrderAsync_ShouldReturnFalse_WhenStatusNotPending()
        {
            var orderId = Guid.NewGuid();
            var order = new Order { Id = orderId, Status = "Shipped" };
            _orderRepo.Setup(r => r.GetByIdAsync(orderId)).ReturnsAsync(order);

            var result = await _service.CancelOrderAsync(orderId);

            Assert.False(result);
        }
    }
}