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
                                    ui.SettingsForDiscountCards();
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
                                    ui.BuyCheerfulDiscountCard(buyer);
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
