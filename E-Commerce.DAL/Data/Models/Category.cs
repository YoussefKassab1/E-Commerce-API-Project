namespace E_Commerce.DAL.Data.Models
{
    public class Category : IAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

        // Audit Fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}