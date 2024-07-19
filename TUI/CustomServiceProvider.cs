using Core.Data;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TUI;

namespace Core
{
    public class CustomServiceProvider
    {
        public static IServiceCollection BuildServiceProvider()
        {
            var services = new ServiceCollection();
            
            services.AddTransient<DiscountCardService>();
            services.AddTransient<UI>();
            services.AddTransient<IFileLoggerService, FileLoggerService>();
            services.AddTransient(typeof(ILogger<>), typeof(CustomLogger<>));
            services.AddTransient<IRepository<Product>, ProductRepository>(_ => new ProductRepository(DB.products));
            services.AddTransient<IShoppingCartRepository, ShoppingCartRepository>(_ => new ShoppingCartRepository(DB.shoppingCarts));
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient(_ => new UserRepository(DB.users));
            services.AddTransient<IDiscountCardService, DiscountCardService>();
            services.AddTransient<IAuthentication, Authentication>();

            return services;
        }
    }
}
