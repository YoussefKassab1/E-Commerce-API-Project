using E_Commerce.DAL.Data.Models;
using E_Commerce.Common;

namespace E_Commerce.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllWithCategoriesAsync();
        Task<Product?> GetByIdWithCategoriesAsync(int id);

        Task<PagedResult<Product>> GetProductsPagination(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters);
    }
}
