using Microsoft.EntityFrameworkCore;
using ShopMate.API.Models;

namespace ShopMate.API.Data
{
    public class ShopMateDbContext : DbContext
    {
        public ShopMateDbContext(
            DbContextOptions<ShopMateDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
    }
}