using Core;
using Core.Data;
using Core.Entities;
using Core.Repositories;
using Core.Services;

namespace TUI
{
    public class UI
    {
        private readonly IProductRepository _repository;
        private readonly IShoppingCartService _shoppingCartService;

        public UI(IProductRepository repository, IShoppingCartService shoppingCartService)
        {
            _repository = repository;
            _shoppingCartService = shoppingCartService;
        }

        public User Authentication()
        {
            string message = $"Авторизуйтесь{Environment.NewLine}Логин: ";
            Display(message);
            string login = DataInput();
            Display("Пароль: ");
            string password = HiddenPasswordInput();

            Authentication authentication
                = new Authentication(new SellerRepository(DB.users));

            var user = authentication.Authenticate(login, password);
            message = user.IsAuthenticated ?
                $"{Environment.NewLine}Здравствуйте {user.Name}, Вы авторизованы!{Environment.NewLine}" :
                $"{Environment.NewLine}Вы не авторизовались!{Environment.NewLine}";

            Display(message);
            Clear(2000);
            return user;
        }

        public string Menu(User user)
        {
            string message = string.Empty;

            if (user.Role == Role.Buyer)
            {
                message = """
                        1.Выбрать товар
                        2.Оплата
                        3.Корзина
                        4.Разлогиниться

                        """;
            }
            else if (user.Role == Role.Seller)
            {
                message = """
                        1.Добавить товар
                        2.Удалить товар
                        3.Разлогиниться

                        """;
            }

            Display(message);
            string value = DataInput();
            Clear(0);
            return value;
        }

        public void Payment()
        {

        }

        public void SelectProducts(Buyer buyer)
        {
            var enteredValue = string.Empty;
            while (!string.Equals(enteredValue.ToLower(), "q"))
            {
                var products = _repository.GetAll();
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                Display("Выбери товар в корзину: ");
                enteredValue = DataInput();
                int.TryParse(enteredValue, out int valueId);
                _shoppingCartService.AddProduct(buyer, valueId);
                Clear(0);
            }
            Clear(0);
        }

        public void ShowCart(Buyer buyer)
        {
            var enteredValue = string.Empty;
            var message = string.Empty;
            while (!string.Equals(enteredValue.ToLower(), "q"))
            {
                var products = buyer.ShoppingCart;
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                message = $"""
                         1.Оплатить товар
                         2.Изменить количество в каждой позиции
                         3.Для выхода 'q'

                         Выбери действие: 
                         """;
                Display(message);
                enteredValue = DataInput();
                int.TryParse(enteredValue, out int positionNumber);
                if (positionNumber == 1)
                {
                    Payment();
                }
                else if (positionNumber == 2)
                {
                    Display("Введи ID товара: ");
                    var indexProduct = DataInput();
                    int.TryParse(indexProduct, out int productId);
                    Display("Введи количество: ");
                    var quantityString = DataInput();
                    int.TryParse(quantityString, out int quantity);
                    _shoppingCartService.UpdateQuantityProduct(buyer, productId, quantity);
                }
                Clear(0);
            }
            Clear(0);
        }

        public void AddProduct()
        {

        }

        public void RemoveProduct()
        {

        }

        public void SignOut()
        {

        }

        private string DataInput()
        {
            string data = Console.ReadLine();
            if (string.IsNullOrEmpty(data))
            {
                throw new Exception("Please, enter the required data");
            }
            return data;
        }

        private void Display(string message)
        {
            Console.Write(message);
        }

        private void Clear(int timeout)
        {
            Thread.Sleep(timeout);
            Console.Clear();
        }

        private string HiddenPasswordInput()
        {
            string password = string.Empty;
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                password += key.KeyChar;
            }
            return password;
        }
    }
}
