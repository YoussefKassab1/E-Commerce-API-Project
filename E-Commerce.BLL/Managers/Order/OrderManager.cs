using E_Commerce.Common;
using E_Commerce.DAL;
using E_Commerce.DAL.Data.Models;
using E_Commerce.DAL.Data.Models.Enums;

namespace E_Commerce.BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderManager(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<GeneralResult<OrderDetailsDto>> PlaceOrderAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart is null || !cart.CartItems.Any())
                return GeneralResult<OrderDetailsDto>.Failure("Your cart is empty.");

            var orderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product?.Price ?? 0,
            }).ToList();

            var order = new Order
            {
                UserId = userId,
                Status = OrderStatus.Pending,
                TotalAmount = orderItems.Sum(oi => oi.UnitPrice * oi.Quantity),
                OrderItems = orderItems,
            };

            _unitOfWork.OrderRepository.AddOrder(order);
            _unitOfWork.CartRepository.ClearCart(cart);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<OrderDetailsDto>.Success(
                MapToDetails(order), "Order placed successfully.");
        }

        public async Task<GeneralResult<IEnumerable<OrderSummaryDto>>> GetOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
            var dtos = orders.Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                Status = o.Status.ToString(),
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt,
            });
            return GeneralResult<IEnumerable<OrderSummaryDto>>.Success(dtos);
        }

        public async Task<GeneralResult<OrderDetailsDto>> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId, userId);
            if (order is null)
                return GeneralResult<OrderDetailsDto>.NotFound("Order not found.");

            return GeneralResult<OrderDetailsDto>.Success(MapToDetails(order));
        }

        private static OrderDetailsDto MapToDetails(Order o) => new()
        {
            Id = o.Id,
            Status = o.Status.ToString(),
            TotalAmount = o.TotalAmount,
            CreatedAt = o.CreatedAt,
            Items = o.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Title ?? string.Empty,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                SubTotal = oi.UnitPrice * oi.Quantity,
            }).ToList(),
        };
    }
}
