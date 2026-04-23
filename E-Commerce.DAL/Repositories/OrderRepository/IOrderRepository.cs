using E_Commerce.DAL.Data.Models;

namespace E_Commerce.DAL.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByIdAsync(int orderId , string userId);
        void AddOrder(Order order);
    }
}
