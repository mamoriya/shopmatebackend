namespace ShopMate.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; } = null!;
    }
}