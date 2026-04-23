using E_Commerce.DAL.Repositories.CartRepository;
using E_Commerce.DAL.Repositories.OrderRepository;

namespace E_Commerce.DAL
{
    public interface IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }
        public IOrderRepository OrderRepository { get; }
        Task SaveChangesAsync();
    }
}
