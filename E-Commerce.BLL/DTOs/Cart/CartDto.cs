namespace E_Commerce.BLL
{
    public class CartDto
    {
        public int CartId { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; } = [];
        public decimal Total { get; set; }
    }
}
