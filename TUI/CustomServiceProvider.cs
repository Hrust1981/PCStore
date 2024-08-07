﻿using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Configuration;
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
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddTransient<UI>()
                .AddTransient<IFileLoggerService, FileLoggerService>()
                .AddTransient<IRepository<Product>, ProductRepository>()
                .AddTransient<IShoppingCartRepository, ShoppingCartRepository>()
                .AddTransient<IShoppingCartService, ShoppingCartService>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IDiscountCardService, DiscountCardService>()
                .AddTransient<IAuthentication, Authentication>()
                .AddTransient<IStringLocalizer, CustomLocalizationService>()
                .AddTransient(typeof(ILogger<>), typeof(CustomLogger<>));
            services.AddOptions<LoggerOptions>().Bind(configuration.GetSection(LoggerOptions.ConfigKey));
            services.AddOptions<DiscountCardsOptions>().Bind(configuration.GetSection(DiscountCardsOptions.ConfigKeyQuantumDC));
            services.AddOptions<DiscountCardsOptions>().Bind(configuration.GetSection(DiscountCardsOptions.ConfigKeyCheerfulDC));
            services.AddLocalization(options => options.ResourcesPath = "Properties");
            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
