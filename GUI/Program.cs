using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ServiceProviderOfDI.BuildServiceProvider();
            var productService = serviceProvider.GetService<IProductRepository>();
            var shoppingCartService = serviceProvider.GetService<IShoppingCartService>();
            var userService= serviceProvider.GetService<IUserRepository>();

            UI ui = new UI(productService, shoppingCartService, userService);
            while (true)
            {
                var user = ui.Authentication();
                bool isAuthenticated = user.IsAuthenticated;
                while (isAuthenticated)
                {
                    var menuItem = ui.Menu(user);
                    if (user.Role == Role.Buyer)
                    {
                        switch (menuItem)
                        {
                            case "1":
                                ui.SelectProducts((Buyer)user);
                                break;
                            case "2":
                                ui.ShowCart((Buyer)user);
                                break;
                            case "3":
                                isAuthenticated = false;
                                break;
                            default:
                                break;

                        }
                    }
                    else if (user.Role == Role.Seller)
                    {
                        switch (menuItem)
                        {
                            case "1":
                                ui.AddProduct();
                                break;
                            case "2":
                                ui.RemoveProduct();
                                break;
                            case "3":
                                isAuthenticated = false;
                                break;
                            default:
                                break;

                        }
                    }
                }
            }
        }
    }
}
