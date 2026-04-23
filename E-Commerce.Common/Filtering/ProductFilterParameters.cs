namespace E_Commerce.Common
{
    public class ProductFilterParameters : BaseFilterParameters
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }
}
