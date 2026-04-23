using E_Commerce.DAL.Data.Models;

namespace E_Commerce.DAL
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithProductsAsync();
        Task<Category?> GetByIdWithProductsAsync(int id);
    }
}
