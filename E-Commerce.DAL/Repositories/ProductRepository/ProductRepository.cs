using E_Commerce.Common;
using E_Commerce.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.DAL
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithCategoriesAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoriesAsync(int id)
        {
            return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResult<Product>> GetProductsPagination
            (
            PaginationParameters paginationParameters,
            ProductFilterParameters productFilterParameters
            )
        {
            IQueryable<Product> query = _context.Set<Product>().AsQueryable();

            query = query.Include(p => p.Category);

            if (productFilterParameters != null)
            {
                query = ApplyFilters(query, productFilterParameters);
            }

            var totalCount = await query.CountAsync();

            var pageNumber = paginationParameters?.PageNumber ?? 1;
            var pageSize = paginationParameters?.PageSize ?? totalCount;

            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<Product>
            {
                Items = items,
                MetaData = new PaginationMetadata
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    HasNext = pageNumber < totalPages,
                    HasPrev = pageNumber > 1,
                }
            };
        }

        private IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductFilterParameters productFilterParameters)
        {
            if(productFilterParameters.CategoryId > 0)
            {
                query = query.Where(p => p.CategoryId == productFilterParameters.CategoryId);
            }

            if(!string.IsNullOrEmpty(productFilterParameters.Name))
            {
                query = query.Where(p => p.Title.Contains(productFilterParameters.Name));
            }

            if (productFilterParameters.MinPrice > 0)
            {
                query = query.Where(p => p.Price >= productFilterParameters.MinPrice);
            }

            if (productFilterParameters.MaxPrice > 0)
            {
                query = query.Where(p => p.Price <= productFilterParameters.MaxPrice);
            }

            if (!string.IsNullOrEmpty(productFilterParameters.Search))
            {
                query = query.Where(p => p.Title.Contains(productFilterParameters.Search));
            }

            return query;
        }

    }
}
