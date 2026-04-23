using E_Commerce.Common;

namespace E_Commerce.BLL
{
    public interface IOrderManager
    {
        Task<GeneralResult<OrderDetailsDto>> PlaceOrderAsync(string userId);
        Task<GeneralResult<IEnumerable<OrderSummaryDto>>> GetOrdersAsync(string userId);
        Task<GeneralResult<OrderDetailsDto>> GetOrderByIdAsync(string userId, int orderId);
    }
}
