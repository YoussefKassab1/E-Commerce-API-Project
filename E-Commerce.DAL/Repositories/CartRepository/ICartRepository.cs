using E_Commerce.DAL.Data.Models;

namespace E_Commerce.DAL.Repositories.CartRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId);
        void AddCart(Cart cart);
        void ClearCart(Cart cart);
        void AddCartItem(CartItem cartItem);
        void RemoveCartItem(CartItem cartItem);
    }
}
