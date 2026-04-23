namespace E_Commerce.BLL
{
    public class ProductCreateDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public int CategoryId { get; set; }
    }
}
