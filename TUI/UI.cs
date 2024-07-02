using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace TUI
{
    public class UI
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<UI> _logger;
        private readonly IAuthentication _authentication;

        public UI(IRepository<Product> repository,
                  IShoppingCartService shoppingCartService,
                  IShoppingCartRepository shoppingCartRepository,
                  ILogger<UI> logger,
                  IAuthentication authentication)
        {
            _productRepository = repository;
            _shoppingCartService = shoppingCartService;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _authentication = authentication;
        }

        public void SelectCulture()
        {
            var message = """
                            Выберите язык:
                            1. Русский
                            2. English

                            """;
            var positionNumber = GetEnteredNumericValue(message);
            CultureInfo culture;
            if (positionNumber == 2)
            {
                culture = CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            else
            {
                culture = CultureInfo.CreateSpecificCulture("ru-RU");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            Clear(1500);
        }

        public User Authentication()
        {
            var message = Properties.Strings.Login;
            Display(message);
            string login = DataInput();
            Display("Пароль: ");
            string password = HiddenPasswordInput();

            var user = _authentication.Authenticate(login, password);

            if (user.IsAuthenticated)
            {
                message = $"{Environment.NewLine}Здравствуйте {user.Name}, Вы авторизованы!{Environment.NewLine}";
                _logger.LogInformation($"User with login:{user.Login} is authorized");
            }
            else
            {
                message = $"{Environment.NewLine}Вы не авторизовались!{Environment.NewLine}";
                _logger.LogWarning($"User with login:{user.Login} is not authorized");
            }

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
                        q.Разлогиниться

                        """;
            }
            else if (user.Role == Role.Seller)
            {
                message = """
                        1.Добавить товар
                        2.Удалить товар
                        3.Редактировать продукт
                        q.Разлогиниться

                        """;
            }
            Display(message);
            string value = DataInput();
            Clear(0);
            return value;
        }

        public void Payment(Buyer buyer)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id).Products;
            if (shoppingCart.Count != 0)
            {
                var hasPayment = _shoppingCartService.Payment(buyer, shoppingCart);
                var messsage = hasPayment ? "Оплата прошла успешно!" : "К сожалению оплата не прошла!";
                Display(messsage);
            }
            else
            {
                Display("Корзина пуста!");
            }
            Clear(2500);
        }

        public void SelectProducts(Buyer buyer)
        {
            var productId = 0;
            while (productId != Constants.Exit)
            {
                var products = _productRepository.GetAll();
                if (products.Count == 0)
                {
                    _logger.LogWarning("There are no products in the database");
                }
                var idCounter = 0;
                foreach (var product in products)
                {
                    DisplayLine(++idCounter + product.ToString());
                }

                DisplayLine("Для выхода нажмите клавишу 'q'");
                productId = GetEnteredNumericValue("Выберите товар в корзину: ");

                if (productId > 0)
                {
                    _shoppingCartService.AddProduct(buyer, products[productId - 1]);
                }
                Clear(0);
            }
            Clear(0);
        }

        public void ShowCart(Buyer buyer)
        {
            var positionNumber = 0;

            while (positionNumber != Constants.Exit)
            {
                var products = _shoppingCartRepository.GetByUserId(buyer.Id).Products;
                var idCounter = 0;
                foreach (var product in products)
                {
                    DisplayLine(++idCounter + product.ToString());
                }

                string? message;
                if (products.Count != 0)
                {
                    var totalAmount = _shoppingCartService.CalculateTotalAmount(buyer);
                    message = $"        Сумма к оплате: {totalAmount} рублей";
                    DisplayLine(message);
                }
                message = $"""
                         1.Оплатить товар
                         2.Изменить количество в каждой позиции
                         3.Удалить товар из корзины

                         Для выхода нажмите клавишу 'q'

                         Выберите действие: 
                         """;
                positionNumber = GetEnteredNumericValue(message);
                switch (positionNumber)
                {
                    case Constants.PayGoods:
                        Payment(buyer);
                        break;
                    case Constants.ChangeProductQuantity:
                        {
                            var productId = GetEnteredNumericValue("Введите ID товара: ");
                            var quantity = GetEnteredNumericValue("Введите количество: ");
                            _shoppingCartService.UpdateQuantityProduct(buyer, products[productId - 1], quantity);
                            break;
                        }
                    case Constants.RemoveItemFromCart:
                        {
                            var productId = GetEnteredNumericValue("Введите ID товара: ");
                            _shoppingCartService.DeleteProduct(buyer, products[productId - 1]);
                            break;
                        }
                }
                Clear(0);
            }
            Clear(0);
        }

        public void AddProduct()
        {
            var name = GetEnteredStringValue("Введите название товара");
            var description = GetEnteredStringValue("Введите описание товара");
            var price = GetEnteredNumericValue("Введите цену товара: ");
            var quantity = GetEnteredNumericValue("Введите количество товара: ");

            _productRepository.Add(new Product(name, description, price, quantity));
            _logger.LogInformation($"The new product named '{name}' has been added to the product repository");

            Clear(0);
        }

        public void RemoveProduct()
        {
            var productId = 0;
            while (productId != Constants.Exit)
            {
                var products = _productRepository.GetAll();
                if (products.Count == 0)
                {
                    _logger.LogWarning("There are no products in the database");
                }

                var idCounter = 0;
                foreach (var product in products)
                {
                    DisplayLine(++idCounter + product.ToString());
                }

                DisplayLine("Для выхода нажмите клавишу 'q'");
                productId = GetEnteredNumericValue("Для удаления введите ID товара: ");
                if (productId > 0)
                {
                    var deletetableProduct = products[productId - 1];
                    if (deletetableProduct != null)
                    {
                        _productRepository.Delete(deletetableProduct.Id);
                        _logger.LogInformation($"Product with ID:{productId} has been removed from the product repository");
                    }
                    else
                    {
                        _logger.LogWarning($"Product with ID:{productId} not found");
                        throw new Exception($"Product with ID:{productId} is not found");
                    }
                }
                Clear(0);
            }
            Clear(0);
        }

        public void UpdateProduct()
        {
            var productId = 0;
            while (productId != Constants.Exit)
            {
                var products = _productRepository.GetAll();
                if (products.Count == 0)
                {
                    _logger.LogWarning("There are no products in the database");
                }

                var idCounter = 0;
                foreach (var product in products)
                {
                    DisplayLine(++idCounter + product.ToString());
                }

                DisplayLine("Для выхода нажмите клавишу 'q'");
                productId = GetEnteredNumericValue("Для редактирования введите ID товара: ");

                if (productId > 0)
                {
                    var updatableProduct = products[productId - 1];
                    if (updatableProduct != null)
                    {
                        var enteredName = GetEnteredStringValue("Введите название товара");
                        var name = string.IsNullOrEmpty(enteredName) ? updatableProduct.Name : enteredName;
                        var enteredDescription = GetEnteredStringValue("Введите описание товара");
                        var description = string.IsNullOrEmpty(enteredDescription) ? updatableProduct.Description : enteredDescription;
                        var enteredPrice = GetEnteredNumericValue("Введите цену товара: ");
                        var price = enteredPrice > 0 ? enteredPrice : updatableProduct.Price;
                        var enteredQuantity = GetEnteredNumericValue("Введите количество товара: ");
                        var quantity = enteredQuantity > 0 ? enteredQuantity : updatableProduct.Quantity;

                        _productRepository.Update(new Product(updatableProduct.Id, name, description, price, quantity));
                        _logger.LogInformation($"Product with ID:{updatableProduct.Id} updated");
                    }
                    else
                    {
                        _logger.LogWarning($"Product with ID:{productId} not found");
                        throw new Exception($"Product with ID:{productId} is not found");
                    }
                }
                Clear(0);
            }
            Clear(0);
        }

        public bool SignOut(string login)
        {
            _logger.LogInformation($"User with login:{login} is logged out");
            return false;
        }

        private string GetEnteredStringValue(string message)
        {
            DisplayLine(message);
            return DataInput();
        }

        private int GetEnteredNumericValue(string message)
        {
            Display(message);
            var enteredValue = DataInput();
            if (string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
            {
                return -1;
            }
            if (!int.TryParse(enteredValue, out int valueId))
            {
                _logger.LogError("Invalid data was entered");
            }
            return valueId;
        }

        private string DataInput()
        {
            return Console.ReadLine();
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