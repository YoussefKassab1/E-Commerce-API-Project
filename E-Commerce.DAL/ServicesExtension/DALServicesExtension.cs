using E_Commerce.DAL.Data.Models;
using E_Commerce.DAL.Repositories.CartRepository;
using E_Commerce.DAL.Repositories.OrderRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.DAL
{
    public static class DALServicesExtension
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("E_Commerce");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                .UseAsyncSeeding(async (context, _, _) =>
                {
                    if (await context.Set<Category>().AnyAsync())
                    {
                        return;
                    }

                    if (await context.Set<Product>().AnyAsync())
                    {
                        return;
                    }

                    var categories = SeedDataProvider.GetCategories();
                    var products = SeedDataProvider.GetProducts();

                    await context.AddRangeAsync(categories);
                    await context.AddRangeAsync(products);

                    await context.SaveChangesAsync();
                })
                .UseSeeding((context, _) =>
                {
                    if ( context.Set<Category>().Any())
                    {
                        return;
                    }

                    if ( context.Set<Product>().Any())
                    {
                        return;
                    }

                    var categories = SeedDataProvider.GetCategories();
                    var products = SeedDataProvider.GetProducts();

                    context.AddRange(categories);
                    context.AddRange(products);

                    context.SaveChanges();

                });
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
