using E_Commerce.DAL.Repositories.CartRepository;
using E_Commerce.DAL.Repositories.OrderRepository;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public ICartRepository CartRepository { get; }
        public IOrderRepository OrderRepository { get; }



        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ICartRepository cartRepository,
            IOrderRepository orderRepository)
        {
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            CartRepository = cartRepository;
            OrderRepository = orderRepository;
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
