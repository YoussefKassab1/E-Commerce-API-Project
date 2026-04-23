using E_Commerce.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DAL.Repositories.CartRepository
{
    public class CartRepository : GenericRepository<Cart> , ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci=> ci.CartId == cartId && ci.ProductId == productId);
        }
        public void AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public void AddCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
        }

        public void ClearCart(Cart cart)
        {
            _context.CartItems.RemoveRange(cart.CartItems);
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }
    }
}
