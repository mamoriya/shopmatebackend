using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopMate.API.Data;
using ShopMate.API.Models;

namespace ShopMate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ShopMateDbContext _context;

        public CartController(ShopMateDbContext context)
        {
            _context = context;
        }

        // GET: api/cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cartItems = await _context.CartItems
                .Select(item => new
                {
                    id = item.Id,
                    productId = item.ProductId,
                    createdAt = item.CreatedAt,
                    product = new
                    {
                        id = item.Product.Id,
                        name = item.Product.Name,
                        price = item.Product.Price,
                        image = item.Product.Image
                    }
                })
                .ToListAsync();

            return Ok(cartItems);
        }

        // POST: api/cart
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products
                .FindAsync(productId);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(item => item.ProductId == productId);

            if (existingItem != null)
            {
                return Conflict("Product is already in cart.");
            }

            var cartItem = new CartItem
            {
                ProductId = productId
            };

            _context.CartItems.Add(cartItem);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Product added to cart successfully.",
                cartItemId = cartItem.Id,
                productId = cartItem.ProductId
            });
        }

        // DELETE: api/cart/{productId}
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(item => item.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("Product is not in cart.");
            }

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Product removed from cart successfully.",
                productId = productId
            });
        }
    }
}