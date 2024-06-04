using Core.Data;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core
{
    public class ServiceProviderOfDI
    {
        public static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection()
                .AddTransient<IProductRepository>(_ => new ProductRepository(DB.products))
                .AddTransient<IShoppingCartService>(_ => new ShoppingCartService(new ProductRepository(DB.products),
                                                                                 new CustomLogger()))
                .AddTransient<IUserRepository>(_ => new UserRepository(DB.users));

            return services.BuildServiceProvider();
        }
    }
}
