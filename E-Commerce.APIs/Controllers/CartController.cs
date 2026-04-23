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
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // Get the current user's cart
        [HttpGet]
        public async Task<ActionResult<GeneralResult<CartDto>>> GetCart()
        {
            var result = await _cartManager.GetCartAsync(GetUserId());

            if (!result.IsSuccess)
                return BadRequest(result);
            
            return Ok(result);
        }

        // Add to cart
        [HttpPost]
        public async Task<ActionResult<GeneralResult>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var result = await _cartManager.AddToCartAsync(GetUserId(), addToCartDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Update cart item quantity
        [HttpPut]
        public async Task<ActionResult<GeneralResult>> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
        {
            var result = await _cartManager.UpdateCartItemAsync(GetUserId(), updateCartItemDto);

            if (!result.IsSuccess)
                return BadRequest(result);
            
            return Ok(result);
        }

        // Remove from cart
        [HttpDelete("{productId}")]
        public async Task<ActionResult<GeneralResult>> RemoveFromCart([FromRoute] int productId)
        {
            var result = await _cartManager.RemoveFromCartAsync(GetUserId(), productId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
