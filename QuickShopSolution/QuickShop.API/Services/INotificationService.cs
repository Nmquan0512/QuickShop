using QuickShop.API.Models;

namespace QuickShop.API.Services
{
    public interface INotificationService
    {
        Task SendOrderConfirmationAsync(Order order);
    }
}
