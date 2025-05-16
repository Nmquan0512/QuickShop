namespace QuickShop.API.Services
{
    public interface IProductService
    {
        Task<bool> ProductExistsAsync(string productId);
    }
}
