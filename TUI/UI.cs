using Core;
using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace TUI
{
    public class UI
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<UI> _logger;
        private readonly IAuthentication _authentication;
        private readonly IDiscountCardService _discountCardService;

        public UI(IRepository<Product> repository,
                  IShoppingCartService shoppingCartService,
                  IShoppingCartRepository shoppingCartRepository,
                  ILogger<UI> logger,
                  IAuthentication authentication,
                  IDiscountCardService discountCardService)
        {
            _productRepository = repository;
            _shoppingCartService = shoppingCartService;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _authentication = authentication;
            _discountCardService = discountCardService;
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
            string message = Properties.Strings.Login;
            Display(message);
            string login = DataInput();

            message = Properties.Strings.Password;
            DisplayLine(message);
            string password = HiddenPasswordInput();

            var user = _authentication.Authenticate(login, password);

            if (user.IsAuthenticated)
            {
                message = Properties.Strings.Hello + user.Name + ", " + Properties.Strings.LoggedIn;
                _logger.LogInformation($"User with login:{user.Login} is authorized");
            }
            else
            {
                message = Properties.Strings.NotLoggedIn;
                _logger.LogWarning($"User with login:{user.Login} is not authorized");
            }

            DisplayLine(message);
            Clear(2000);
            return user;
        }

        public string Menu(User user)
        {
            string message = string.Empty;

            if (user.Role == Role.Admin)
            {
                message = Properties.Strings.MenuForAdmin;
            }
            else if (user.Role == Role.Buyer)
            {
                message = Properties.Strings.MenuForBuyer;
            }
            else if (user.Role == Role.Seller)
            {
                message = Properties.Strings.MenuForSeller;
            }
            Display(message);
            string value = DataInput();
            Clear(0);
            return value;
        }

        public void SetPathForLoggingFile()
        {
            DisplayLine(Properties.Strings.PathToLogFile);
            var path = DataInput();

            if (string.Equals("q", path, StringComparison.OrdinalIgnoreCase))
            {
                Clear(100);
                return;
            }
            CustomConfigurationManager.SetValueByKey("PathToLoggerFile", path);
            Clear(100);
        }

        public void SettingsForDiscountCards()
        {
            var positionNumber = 0;
            while (positionNumber != Constants.Exit)
            {
                var message = Properties.Strings.DiscountCardSettingsMenu;
                positionNumber = GetEnteredNumericValue(message);
                
                if (positionNumber == 1)
                {
                    Clear(100);
                    DisplayLine(Properties.Strings.SpecialDayForQuantumDiscountCard);

                    var date = _discountCardService.GenerateDate();
                    var path = CustomConfigurationManager.GetValueByKey("PathToSettingsForIssueDiscountCards");
                    //var dateIssue = (key:"DateIssueForQuantumDiscountCard", value:date.ToString());
                    DateIssue dateIssue = new DateIssue("DateIssueForQuantumDiscountCard", date);

                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        var json = JsonSerializer.Serialize(dateIssue);
                        byte[] bytes = new UTF8Encoding(true).GetBytes(json);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    Clear(2500);
                }

                Clear(0);
            }
        }

        class DateIssue
        {
            public DateIssue(string key, DateOnly value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; }
            public DateOnly Value { get; set; }
        }

        public void Payment(Buyer buyer)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id).Products;
            var messsage = string.Empty;

            if (shoppingCart.Count != 0)
            {
                var hasPayment = _shoppingCartService.Payment(buyer, shoppingCart);
                messsage = hasPayment ? Properties.Strings.SuccessfulPayment : Properties.Strings.SuccessfulNotPayment;
                Display(messsage);
            }
            else
            {
                messsage = Properties.Strings.CartIsEmpty;
                Display(messsage);
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

                var message = Properties.Strings.Exit;
                DisplayLine(message);

                message = Properties.Strings.AddItemToCart;
                productId = GetEnteredNumericValue(message);

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

                    message = Properties.Strings.AmountToPay + totalAmount + Properties.Strings.RUB;
                    DisplayLine(message);
                }

                message = Properties.Strings.MenuShowCart;
                positionNumber = GetEnteredNumericValue(message);

                message = Properties.Strings.EnterProductID;
                switch (positionNumber)
                {
                    case Constants.PayGoods:
                        Payment(buyer);
                        break;
                    case Constants.ChangeProductQuantity:
                        {
                            var productId = GetEnteredNumericValue(message);

                            message = Properties.Strings.EnterQuantity;
                            var quantity = GetEnteredNumericValue(message);

                            _shoppingCartService.UpdateQuantityProduct(buyer, products[productId - 1], quantity);
                            break;
                        }
                    case Constants.RemoveItemFromCart:
                        {
                            var productId = GetEnteredNumericValue(message);
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
            var message = Properties.Strings.EnterProductName;
            var name = GetEnteredStringValue(message);

            message = Properties.Strings.EnterProductDescription;
            var description = GetEnteredStringValue(message);

            message = Properties.Strings.EnterPriceProduct;
            var price = GetEnteredNumericValue(message);

            message = Properties.Strings.EnterQuantityProduct;
            var quantity = GetEnteredNumericValue(message);

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

                var message = Properties.Strings.Exit;
                DisplayLine(message);

                message = Properties.Strings.ToDeleteEnterProductID;
                productId = GetEnteredNumericValue(message);
                
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

                var message = Properties.Strings.Exit;
                DisplayLine(message);

                message = Properties.Strings.ToEditEnterProductID;
                productId = GetEnteredNumericValue(message);

                if (productId > 0)
                {
                    var updatableProduct = products[productId - 1];
                    if (updatableProduct != null)
                    {
                        message = Properties.Strings.EnterProductName;
                        var enteredName = GetEnteredStringValue(message);
                        var name = string.IsNullOrEmpty(enteredName) ? updatableProduct.Name : enteredName;

                        message = Properties.Strings.EnterProductDescription;
                        var enteredDescription = GetEnteredStringValue(message);
                        var description = string.IsNullOrEmpty(enteredDescription) ? updatableProduct.Description : enteredDescription;

                        message = Properties.Strings.EnterPriceProduct;
                        var enteredPrice = GetEnteredNumericValue(message);
                        var price = enteredPrice > 0 ? enteredPrice : updatableProduct.Price;

                        message = Properties.Strings.EnterQuantityProduct;
                        var enteredQuantity = GetEnteredNumericValue(message);
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

        private string DataInput() => Console.ReadLine();

        private void Display(string message) => Console.Write(message);

        private void DisplayLine(string message) => Console.WriteLine(message);

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