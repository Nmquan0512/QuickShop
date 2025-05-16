using QuickShop.API.Models;

namespace QuickShop.API.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order> GetByIdAsync(Guid id);
        Task UpdateAsync(Order order);
    }
}
