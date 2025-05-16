namespace QuickShop.API.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
