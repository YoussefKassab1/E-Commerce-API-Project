using System.Linq.Expressions;

namespace E_Commerce.DAL
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllGenericAsync
            (
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool trackChanges = false
            );
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        void Insert(T entity);
        void Delete(T entity);
    }
}
