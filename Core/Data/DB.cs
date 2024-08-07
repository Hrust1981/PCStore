﻿using Core.Entities;
using Core.Enumerations;

namespace Core.Data
{
    public class DB
    {
        private static int _counterUserId;

        public static List<User> users = new List<User>()
        {
            new Seller("Seller1", "Seller1", "Seller1", "seller1@mailru", Role.Seller),
            new Seller("Seller2", "Seller2", "Seller2", "seller2@mailru", Role.Seller),
            new Buyer("Buyer1", "Buyer1", "Buyer1", "buyer1@mail.ru", Role.Buyer),
            new Admin("Admin", "Admin", "Admin", "admin@mail.ru", Role.Admin)
        };

        public static List<Product> products = new List<Product>()
        { 
            new Product("PC DEXP Aquilon O296", "Intel Core i3-10100, 4 x 3.6 ГГц, 8 ГБ DDR4, SSD 256 ГБ", 22199, 3),
            new Product("Notebook Lenovo ThinkPad L390", "Intel Core i5-8265U, ядра: 4 х 1.6 ГГц, RAM 8 ГБ", 82499, 4),
            new Product("Monitor Xiaomi A22i", "1920x1080 (FullHD)@75 Гц, VA, LED", 8999, 5),
            new Product("Printer Brother HL-1210W", "A4, 2400x600 dpi, USB, Wi-Fi", 9099, 2)
        };

        public static List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();

        static DB()
        {
            foreach (var user in users)
            {
                if (user is Buyer && !shoppingCarts.Any(s => s.UserId == user.Id))
                {
                    shoppingCarts.Add(new ShoppingCart(new List<Product>(), user.Id));
                }
            }
        }

        public static int CounterUserId { get => ++_counterUserId; }
    }
}
