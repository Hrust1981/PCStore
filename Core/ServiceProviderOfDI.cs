using Core.Data;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public class ServiceProviderOfDI
    {
        public static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection()
                .AddTransient<IProductRepository>(_ => new ProductRepository(DB.products))
                .AddTransient<IShoppingCartService>(_ => new ShoppingCartService(new ProductRepository(DB.products)));
            return services.BuildServiceProvider();
        }
    }
}
