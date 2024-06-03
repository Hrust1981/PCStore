using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace TUI
{
    public class UI
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserRepository _userRepository;
        //private readonly ILogger _logger;

        public UI(IProductRepository repository,
                  IShoppingCartService shoppingCartService,
                  IUserRepository userRepository)
        {
            _productRepository = repository;
            _shoppingCartService = shoppingCartService;
            _userRepository = userRepository;
            //_logger = logger;
        }

        public User Authentication()
        {
            string message = $"Авторизуйтесь{Environment.NewLine}Логин: ";
            Display(message);
            string login = DataInput();
            Display("Пароль: ");
            string password = HiddenPasswordInput();

            var users = _userRepository.GetAll();
            Authentication authentication
                = new Authentication(new SellerRepository(users));

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
                        2.Корзина
                        3.Разлогиниться

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

        public void Payment(Buyer buyer)
        {
            if (buyer.ShoppingCart.Any())
            {
                Display("Оплата прошла успешно!");
                buyer.ShoppingCart.Clear();
                _shoppingCartService.GetQuantityInStock.Clear();
            }
            else
            {
                Display("Корзина пуста!");
            }
            Clear(2500);
        }

        public void SelectProducts(Buyer buyer)
        {
            var enteredValue = string.Empty;
            while (!string.Equals(enteredValue.ToLower(), "q"))
            {
                var products = _productRepository.GetAll();
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                DisplayLine("Для выхода нажите клавишу 'q'");
                var valueId = GetEnteredNumericValue("Выбери товар в корзину: ", out enteredValue);
                _shoppingCartService.AddProduct(buyer, valueId);
                Clear(0);
            }
            Clear(0);
        }

        public void ShowCart(Buyer buyer)
        {
            var enteredValue = string.Empty;
            var message = string.Empty;
            while (!string.Equals(enteredValue.ToLower(), "4"))
            {
                var products = buyer.ShoppingCart;
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                if (products.Any())
                {
                    var sum = buyer.ShoppingCart.Sum(x => x.Price * x.Quantity);
                    message = $"        Сумма к оплате: {sum} рублей";
                    DisplayLine(message);
                }
                message = $"""
                         1.Оплатить товар
                         2.Изменить количество в каждой позиции
                         3.Удалить товар из корзины
                         4.Выход

                         Выбери действие: 
                         """;
                var positionNumber = GetEnteredNumericValue(message, out enteredValue);
                if (positionNumber == 1)
                {
                    Payment(buyer);
                }
                else if (positionNumber == 2)
                {
                    var productId = GetEnteredNumericValue("Введи ID товара: ");
                    var quantity = GetEnteredNumericValue("Введи количество: ");
                    _shoppingCartService.UpdateQuantityProduct(buyer, productId, quantity);
                }
                else if (positionNumber == 3)
                {
                    var productId = GetEnteredNumericValue("Введи ID товара: ");
                    _shoppingCartService.DeleteProduct(buyer, productId);
                }
                Clear(0);
            }
            Clear(0);
        }

        public void AddProduct()
        {
            var name = GetEnteredStringValue("Введи название товара");
            var decription = GetEnteredStringValue("Введи описание товара");
            var price = GetEnteredNumericValue("Введи цену товара: ");
            var quantity = GetEnteredNumericValue("Введи количество товара: ");
            _productRepository.Add(new Product(name, decription, price, quantity));
            Clear(0);
        }

        public void RemoveProduct()
        {
            var enteredValue = string.Empty;
            while (!string.Equals(enteredValue.ToLower(), "q"))
            {
                var products = _productRepository.GetAll();
                foreach (var product in products)
                {
                    DisplayLine(product.ToString());
                }
                DisplayLine("Для выхода нажите клавишу 'q'");
                var productId = GetEnteredNumericValue("Для удаления введи ID товара: ", out enteredValue);
                if (productId > 0)
                {
                    _productRepository.Delete(productId);
                }
                Clear(0);
            }
            Clear(0);
        }

        private string GetEnteredStringValue(string message)
        {
            DisplayLine(message);
            return DataInput();
        }

        private int GetEnteredNumericValue(string message, out string enteredValue)
        {
            Display(message);
            enteredValue = DataInput();
            int.TryParse(enteredValue, out int valueId);
            return valueId;
        }

        private int GetEnteredNumericValue(string message)
        {
            Display(message);
            var enteredValue = DataInput();
            int.TryParse(enteredValue, out int valueId);
            return valueId;
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

        private void DisplayLine(string message)
        {
            Console.WriteLine(message);
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
