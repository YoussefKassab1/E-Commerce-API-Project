using E_Commerce.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DAL.Repositories.OrderRepository
{
    public class OrderRepository : GenericRepository<Order> , IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Where(o=> o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync(); 
        }
        public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o=> o.OrderItems)
                .ThenInclude(oi=> oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
        }
    }
}
