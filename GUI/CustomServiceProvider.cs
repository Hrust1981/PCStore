using Core.Data;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TUI;

namespace Core
{
    public class CustomServiceProvider
    {
        public static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection()
                .AddTransient<UI>()
                .AddTransient<IFileLoggerService, FileLoggerService>()
                .AddTransient<ILogger<ShoppingCartService>, CustomLogger<ShoppingCartService>>()
                .AddTransient<ILogger<UI>, CustomLogger<UI>>()
                .AddTransient(_ => new ProductRepository(DB.products))
                .AddTransient<IShoppingCartService, ShoppingCartService>()
                .AddTransient(_ => new UserRepository(DB.users))
                .AddTransient<IDiscountCardService, DiscountCardService>();
            return services.BuildServiceProvider();
        }
    }
}
