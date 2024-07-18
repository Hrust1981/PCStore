using Core;
using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace TUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = "C:\\Users\\SharipovRR\\source\\repos\\PCStore\\TUI\\appsettings.json";

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path, optional: false)
                .Build();

            var serviceCollection = CustomServiceProvider.BuildServiceProvider();
            serviceCollection.AddOptions<LoggerOptions>().Bind(configuration.GetSection(LoggerOptions.ConfigKey));
            serviceCollection.AddOptions<DiscountCardsOptions>().Bind(configuration.GetSection(DiscountCardsOptions.ConfigKeyQuantumDC));
            serviceCollection.AddOptions<DiscountCardsOptions>().Bind(configuration.GetSection(DiscountCardsOptions.ConfigKeyCheerfulDC));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var ui = serviceProvider.GetRequiredService<UI>();


            while (true)
            {
                ui.SelectCulture();

                var user = ui.Authentication();
                bool isAuthenticated = user.IsAuthenticated;
                
                while (isAuthenticated)
                {
                    var menuItem = ui.Menu(user);
                    switch (user.Role)
                    {
                        case Role.Admin:
                            switch (menuItem)
                            {
                                case Constants.PathLoggingFile:
                                    ui.ShowPathForLoggingFile();
                                    break;
                                case Constants.SettingsDiscountCards:
                                    ui.SettingsForDiscountCardsAsync();
                                    break;
                                case Constants.SignOut:
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                            }
                            break;
                        case Role.Buyer:
                            var buyer = (Buyer)user;
                            switch (menuItem)
                            {
                                case Constants.SelectProducts:
                                    ui.SelectProducts(buyer);
                                    break;
                                case Constants.ShowCart:
                                    ui.ShowCart(buyer);
                                    break;
                                case Constants.BuyCheerfulDiscountCard:
                                    ui.BuyCheerfulDiscountCardAsync(buyer);
                                    break;
                                case Constants.SignOut:
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                            }
                            break;
                        case Role.Seller:
                            switch (menuItem)
                            {
                                case Constants.AddProduct:
                                    ui.AddProduct();
                                    break;
                                case Constants.RemoveProduct:
                                    ui.RemoveProduct();
                                    break;
                                case Constants.UpdateProduct:
                                    ui.UpdateProduct();
                                    break;
                                case Constants.SignOut:
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

    }
}
