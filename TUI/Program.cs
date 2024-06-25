using Core;
using Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace TUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CustomServiceProvider.BuildServiceProvider();
            var ui = serviceProvider.GetRequiredService<UI>();

            while (true)
            {
                var user = ui.Authentication();
                bool isAuthenticated = user.IsAuthenticated;
                while (isAuthenticated)
                {
                    var menuItem = ui.Menu(user);
                    switch (user.Role)
                    {
                        case Role.Buyer:
                            var buyer = (Buyer)user;
                            switch (menuItem)
                            {
                                case "1":
                                    ui.SelectProducts(buyer);
                                    break;
                                case "2":
                                    ui.ShowCart(buyer);
                                    break;
                                case "3":
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                                default:
                                    break;

                            }
                            break;
                        case Role.Seller:
                            switch (menuItem)
                            {
                                case "1":
                                    ui.AddProduct();
                                    break;
                                case "2":
                                    ui.RemoveProduct();
                                    break;
                                case "3":
                                    ui.UpdateProduct();
                                    break;
                                case "4":
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                                default:
                                    break;

                            }
                            break;
                    }
                }
            }
        }
    }
}
