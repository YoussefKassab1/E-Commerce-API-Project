using E_Commerce.Common;
using E_Commerce.DAL;
using E_Commerce.DAL.Data.Models;
using FluentValidation;

namespace E_Commerce.BLL
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddToCartDto> _addValidator;
        private readonly IValidator<UpdateCartItemDto> _updateValidator;

        public CartManager(
            IUnitOfWork unitOfWork,
            IValidator<AddToCartDto> addValidator,
            IValidator<UpdateCartItemDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        // Retrieves the cart for a given user. If the cart is empty, returns an empty CartDto with a message.
        public async Task<GeneralResult<CartDto>> GetCartAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart is null)
                return GeneralResult<CartDto>.Success(new CartDto(), "Cart is empty.");

            return GeneralResult<CartDto>.Success(MapToDto(cart));
        }

        public async Task<GeneralResult> AddToCartAsync(string userId, AddToCartDto dto)
        {
            var validation = await _addValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return GeneralResult.Failure(ToErrorDict(validation));

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
            if (product is null)
                return GeneralResult.NotFound("Product not found.");

            if (product.Count < dto.Quantity)
                return GeneralResult.Failure($"Only {product.Count} units available in stock.");

            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                _unitOfWork.CartRepository.AddCart(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            var existing = await _unitOfWork.CartRepository.GetCartItemAsync(cart.Id, dto.ProductId);
            if (existing is not null)
                existing.Quantity += dto.Quantity;
            else
                _unitOfWork.CartRepository.AddCartItem(new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                });

            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.Success("Item added to cart.");
        }

        public async Task<GeneralResult> UpdateCartItemAsync(string userId, UpdateCartItemDto dto)
        {
            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                return GeneralResult.Failure(ToErrorDict(validation));

            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart is null)
                return GeneralResult.NotFound("Cart not found.");

            var item = await _unitOfWork.CartRepository.GetCartItemAsync(cart.Id, dto.ProductId);
            if (item is null)
                return GeneralResult.NotFound("Item not found in cart.");

            if (dto.Quantity <= 0)
            {
                _unitOfWork.CartRepository.RemoveCartItem(item);
                await _unitOfWork.SaveChangesAsync();
                return GeneralResult.Success("Item removed from cart.");
            }

            item.Quantity = dto.Quantity;
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.Success("Cart updated.");
        }

        public async Task<GeneralResult> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart is null)
                return GeneralResult.NotFound("Cart not found.");

            var item = await _unitOfWork.CartRepository.GetCartItemAsync(cart.Id, productId);
            if (item is null)
                return GeneralResult.NotFound("Item not found in cart.");

            _unitOfWork.CartRepository.RemoveCartItem(item);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.Success("Item removed from cart.");
        }

        private static CartDto MapToDto(Cart cart)
        {
            var items = cart.CartItems.Select(ci => new CartItemDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Title ?? string.Empty,
                UnitPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity,
                SubTotal = (ci.Product?.Price ?? 0) * ci.Quantity,
            }).ToList();

            return new CartDto
            {
                CartId = cart.Id,
                Items = items,
                Total = items.Sum(i => i.SubTotal),
            };
        }

        private static Dictionary<string, List<string>> ToErrorDict(
            FluentValidation.Results.ValidationResult v)
            => v.Errors.GroupBy(e => e.PropertyName)
                       .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());
    }
}
