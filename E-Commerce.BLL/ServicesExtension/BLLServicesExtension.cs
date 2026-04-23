using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.BLL
{
    public static class BLLServicesExtension
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddValidatorsFromAssembly(typeof(BLLServicesExtension).Assembly);
            services.AddAutoMapper(typeof(BLLServicesExtension).Assembly);
        }
    }
}
