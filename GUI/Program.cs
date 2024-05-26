using Core;
using Core.Data;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace TUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            UI ui = new UI(new ProductRepository(DB.products),
                           new ShoppingCartService());
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
                                ui.ShowProducts((Buyer)user);
                                break;
                            case "2":
                                ui.Payment();
                                break;
                            case "3":
                                ui.ShowCart((Buyer)user);
                                break;
                            case "4":
                                ui.SignOut();
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
                                ui.SignOut();
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
