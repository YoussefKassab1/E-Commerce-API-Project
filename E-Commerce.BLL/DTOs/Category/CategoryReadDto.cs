using System.ComponentModel;

namespace E_Commerce.BLL
{
    public class CategoryReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayName("Products Count")]
        public int ProductsCount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
