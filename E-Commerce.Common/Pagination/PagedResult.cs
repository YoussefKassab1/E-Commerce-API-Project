namespace E_Commerce.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = [];

        public PaginationMetadata MetaData { get; set; } = new();
    }
}
