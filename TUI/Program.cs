using Core;
using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using Core.Enumerations;

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
                            switch ((AdminMenu)menuItem)
                            {
                                case AdminMenu.PathLoggingFile:
                                    ui.ShowPathForLoggingFile();
                                    break;
                                case AdminMenu.SettingsDiscountCards:
                                    ui.SettingsForDiscountCards();
                                    break;
                                case AdminMenu.SignOut:
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                            }
                            break;
                        case Role.Buyer:
                            var buyer = (Buyer)user;
                            switch ((BuyerMenu)menuItem)
                            {
                                case BuyerMenu.SelectProducts:
                                    ui.SelectProducts(buyer);
                                    break;
                                case BuyerMenu.ShowCart:
                                    ui.ShowCart(buyer);
                                    break;
                                case BuyerMenu.BuyCheerfulDiscountCard:
                                    ui.BuyCheerfulDiscountCard(buyer);
                                    break;
                                case BuyerMenu.SignOut:
                                    isAuthenticated = ui.SignOut(user.Login);
                                    break;
                            }
                            break;
                        case Role.Seller:
                            switch ((SellerMenu)menuItem)
                            {
                                case SellerMenu.AddProduct:
                                    ui.AddProduct();
                                    break;
                                case SellerMenu.RemoveProduct:
                                    ui.RemoveProduct();
                                    break;
                                case SellerMenu.UpdateProduct:
                                    ui.UpdateProduct();
                                    break;
                                case SellerMenu.SignOut:
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
