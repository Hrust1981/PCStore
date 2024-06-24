using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Logging;

namespace TUI
{
    public class UI
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IRepository<User> _userRepository;
        private readonly IDiscountCardService _discountCardService;
        private readonly ILogger<UI> _logger;
        private readonly IAuthentication _authentication;
        public UI(IRepository<Product> repository,
                  IShoppingCartService shoppingCartService,
                  IRepository<User> userRepository,
                  IDiscountCardService discountCardService,
                  ILogger<UI> logger,
                  IAuthentication authentication)
        {
            _productRepository = repository;
            _shoppingCartService = shoppingCartService;
            _userRepository = userRepository;
            _discountCardService = discountCardService;
            _logger = logger;
            _authentication = authentication;
        }

        public User Authentication()
        {
            string message = $"Авторизуйтесь{Environment.NewLine}Логин: ";
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
                        3.Разлогиниться

                        """;
            }
            else if (user.Role == Role.Seller)
            {
                message = """
                        1.Добавить товар
                        2.Удалить товар
                        3.Редактировать продукт
                        4.Разлогиниться

                        """;
            }

            Display(message);
            string value = DataInput();
            Clear(0);
            return value;
        }

        public void Payment(Buyer buyer)
        {
            var shopingCart = buyer.ShoppingCart.Products;
            if (shopingCart.Any())
            {
                buyer.TotalPurchaseAmount += _shoppingCartService.
                    CalculateTotalAmount(buyer, out int totalAmountWithDiscount);

                var totalAmount = totalAmountWithDiscount == 0 ?
                    shopingCart.Sum(p => p.Price) :
                    totalAmountWithDiscount;

                _discountCardService.AddDiscountCard(buyer);
                Display("Оплата прошла успешно!");
                shopingCart.Clear();
                _shoppingCartService.GetQuantityInStock.Clear();
                _logger.LogInformation($"The goods were paid for in the amount of {totalAmount} RUB by the user {buyer.Login}");
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
            while (!string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
            {
                var products = _productRepository.GetAll();

                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                DisplayLine("Для выхода нажите клавишу 'q'");
                var productId = GetEnteredNumericValue("Выбери товар в корзину: ", out enteredValue);

                if (string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
                {
                    Clear(0);
                    return;
                }

                var productGuid = products.FirstOrDefault(p => p.IntId == productId)?.Id;
                _shoppingCartService.AddProduct(buyer, productGuid.Value);
                Clear(0);
            }
            Clear(0);
        }

        public void ShowCart(Buyer buyer)
        {
            var enteredValue = string.Empty;
            var message = string.Empty;
            while (!string.Equals(enteredValue, "4"))
            {
                var products = buyer.ShoppingCart.Products;
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                if (products.Any())
                {
                    var sum = _shoppingCartService.CalculateTotalAmount(buyer, out int total);
                    var amountToBePaid = total == 0 ? sum : total;
                    message = $"        Сумма к оплате: {amountToBePaid} рублей";
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
                    var productGuid = products.FirstOrDefault(guid => guid.IntId == productId)?.Id;
                    _shoppingCartService.UpdateQuantityProduct(buyer, productGuid.Value, quantity);
                }
                else if (positionNumber == 3)
                {
                    var productId = GetEnteredNumericValue("Введи ID товара: ");
                    var productGuid = products.FirstOrDefault(guid => guid.IntId == productId)?.Id;
                    _shoppingCartService.DeleteProduct(buyer, productGuid.Value);
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
            _logger.LogInformation($"The new product named '{name}' has been added to the product repository");

            Clear(0);
        }

        public void RemoveProduct()
        {
            var enteredValue = string.Empty;
            while (!string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
            {
                var products = _productRepository.GetAll();

                foreach (var product in products)
                {
                    DisplayLine(product.ToString());
                }
                DisplayLine("Для выхода нажите клавишу 'q'");
                var productId = GetEnteredNumericValue("Для удаления введи ID товара: ", out enteredValue);

                if (string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
                {
                    Clear(0);
                    return;
                }

                var productGuid = products.FirstOrDefault(p => p.IntId == productId)?.Id;
                _productRepository.Delete(productGuid.Value);
                _logger.LogInformation($"Product with ID:{productId} has been removed from the product repository");

                Clear(0);
            }
            Clear(0);
        }

        public void UpdateProduct()
        {
            var enteredValue = string.Empty;
            while (!string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
            {
                var products = _productRepository.GetAll();

                foreach (var product in products)
                {
                    DisplayLine(product.ToString());
                }
                DisplayLine("Для выхода нажите клавишу 'q'");

                var productId = GetEnteredNumericValue("Для редактирования введи ID товара: ", out enteredValue);

                if (string.Equals(enteredValue, "q", StringComparison.OrdinalIgnoreCase))
                {
                    Clear(0);
                    return;
                }
                var productGuid = products.FirstOrDefault(p => p.IntId == productId)?.Id;
                var updatableProduct = _productRepository.Get(productGuid.Value);

                if (updatableProduct == null)
                {
                    throw new Exception($"Product with ID:{productId} is not found");
                }
                var enteredName = GetEnteredStringValue("Введи название товара");
                var name = string.IsNullOrEmpty(enteredName) ? updatableProduct.Name : enteredName;
                var enteredDescription = GetEnteredStringValue("Введи описание товара");
                var decription = string.IsNullOrEmpty(enteredDescription) ? updatableProduct.Description : enteredDescription;
                var enteredPrice = GetEnteredNumericValue("Введи цену товара: ");
                var price = enteredPrice > 0 ? enteredPrice : updatableProduct.Price;
                var enteredQuantity = GetEnteredNumericValue("Введи количество товара: ");
                var quantity = enteredQuantity > 0 ? enteredQuantity : updatableProduct.Quantity;

                _productRepository.Update(new Product(updatableProduct.Id, name, decription, price, quantity));
                _logger.LogInformation($"Product with ID:{updatableProduct.Id} updated");

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
