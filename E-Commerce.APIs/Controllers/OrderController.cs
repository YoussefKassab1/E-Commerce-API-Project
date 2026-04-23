using E_Commerce.BLL;
using E_Commerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserAccess")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // Get all orders for the user
        [HttpGet]
        public async Task<ActionResult<GeneralResult<IEnumerable<OrderSummaryDto>>>> GetOrders()
        {
            var result = await _orderManager.GetOrdersAsync(GetUserId());
            return Ok(result);
        }

        // Get order details by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralResult<OrderDetailsDto>>> GetOrderById([FromRoute] int id)
        {
            var result = await _orderManager.GetOrderByIdAsync(GetUserId(), id);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        // Place a new order
        [HttpPost]
        public async Task<ActionResult<GeneralResult<OrderDetailsDto>>> PlaceOrder()
        {
            var result = await _orderManager.PlaceOrderAsync(GetUserId());

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
