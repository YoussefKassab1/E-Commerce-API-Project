using E_Commerce.Common;

namespace E_Commerce.BLL
{
    public interface ICartManager
    {
        Task<GeneralResult<CartDto>> GetCartAsync(string userId);
        Task<GeneralResult> AddToCartAsync(string userId, AddToCartDto dto);
        Task<GeneralResult> UpdateCartItemAsync(string userId, UpdateCartItemDto dto);
        Task<GeneralResult> RemoveFromCartAsync(string userId, int productId);
    }
}
