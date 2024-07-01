﻿using Core.Data;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
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
                .AddTransient(typeof(ILogger<>), typeof(CustomLogger<>))
                .AddTransient<IRepository<Product>, ProductRepository>(_ => new ProductRepository(DB.products))
                .AddTransient<IShoppingCartRepository, ShoppingCartRepository>(_ => new ShoppingCartRepository(DB.shoppingCarts))
                .AddTransient<IShoppingCartService, ShoppingCartService>()
                .AddTransient(_ => new UserRepository(DB.users))
                .AddTransient<IDiscountCardService, DiscountCardService>()
                .AddTransient<IAuthentication, Authentication>()
                .AddTransient<IStringLocalizer<UI>, StringLocalizer<UI>>()
                .AddLocalization(options => options.ResourcesPath = "TUI.Properties")
                .AddLogging();
            return services.BuildServiceProvider();
        }
    }
}
