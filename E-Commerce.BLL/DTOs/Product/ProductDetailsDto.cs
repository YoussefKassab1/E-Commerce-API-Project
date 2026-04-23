namespace E_Commerce.BLL
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}
