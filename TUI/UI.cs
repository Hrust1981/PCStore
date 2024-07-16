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
        private readonly IAuthentication _authentication;
        private readonly IDiscountCardService _discountCardService;
        private readonly ILogger<UI> _logger;

        public UI(IRepository<Product> repository,
                  IShoppingCartService shoppingCartService,
                  IShoppingCartRepository shoppingCartRepository,
                  IAuthentication authentication,
                  IDiscountCardService discountCardService,
                  ILogger<UI> logger)
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
            try
            {
                var message = Properties.Strings.SelectLanguage;
                var positionNumber = (Culture)GetEnteredNumericValue(message);

                if (positionNumber == Culture.Russian)
                {
                    SetCulture("ru-RU");
                }
                else if (positionNumber == Culture.English)
                {
                    SetCulture("en-US");
                }
                else
                {
                    SetCulture("ru-RU");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in the SelectCulture method. Message - {ex.Message}");
            }
            Clear(1500);
        }

        public User Authentication()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the Authentication method. Message - {ex.Message}");
                Clear(100);
                return User.Empty;
            }
        }

        public string Menu(User user)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the Menu method. Message - {ex.Message}");
                Clear(100);
                return string.Empty;
            }
        }

        public async Task BuyCheerfulDiscountCard(Buyer buyer)
        {
            try
            {
                var message = string.Empty;
                var discountCards = buyer.DiscountCards;

                if (!discountCards.Any(dc => string.Equals(dc.Name, "CheerfulDiscountCard")))
                {
                    discountCards.Add(await CheerfulDiscountCard.CreateAsync());
                    message = Properties.Strings.CheerfulDiscountCardPurchased;
                }
                else
                {
                    message = Properties.Strings.CheerfulDiscountCardAlreadyPurchased;
                }
                Display(message);
                Clear(2500);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the BuyCheerfulDiscountCard method. Message - {ex.Message}");
            }
            Clear(100);
        }

        public void ShowPathForLoggingFile()
        {
            try
            {
                DisplayLine(Properties.Strings.PathToLogFile);
                var path = CustomConfigurationManager.GetValueByKey("PathToLoggerFile");
                Display(path);
                Clear(2500);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the ShowPathForLoggingFile method. Message - {ex.Message}");
            }
            Clear(100);
        }

        public async Task SettingsForDiscountCards()
        {
            try
            {
                var positionNumber = 0;
                while (positionNumber != Constants.Exit)
                {
                    var message = Properties.Strings.DiscountCardSettingsMenu;
                    positionNumber = GetEnteredNumericValue(message);

                    if (positionNumber == Constants.DateIssueQuantumDiscountCard)
                    {
                        Clear(100);
                        message = Properties.Strings.SpecialDayForQuantumDiscountCard;
                        DisplayLine(message);

                        var date = await _discountCardService.GenerateDateIssueOrWorkDiscountCardAsync("DateIssue");

                        Display(date.ToString());
                        _logger.LogInformation($"The day for issuing a quantum discount card has been determined - {date}");
                    }
                    else if (positionNumber == Constants.ValidityPeriodQuantumDiscountCard)
                    {
                        Clear(100);
                        message = Properties.Strings.ValidityPeriodQuantumDiscountCard;

                        var numberDays = GetEnteredNumericValue(message);

                        await _discountCardService.SetDayForIssueQuantumDiscountCardAsync(numberDays);
                        _logger.LogInformation($"The validity period of the Quantum discount card has been changed to {numberDays} days");
                    }
                    else if (positionNumber == Constants.WorkDatesCheerfulDiscountCard)
                    {
                        Clear(100);
                        message = Properties.Strings.WorkDatesCheerfulDiscountCard;
                        DisplayLine(message);

                        var date = await _discountCardService.GenerateDateIssueOrWorkDiscountCardAsync("WorkDatesCheerfulDiscountCard", 10);

                        Display(date.ToString());
                        _logger.LogInformation($"The range {date}-{date.AddDays(9)} in days when the Cheerful discount card works is determined");
                    }
                    Clear(2500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the SettingsForDiscountCards method. Message - {ex.Message}");
            }
            Clear(100);
        }

        private async Task Payment(Buyer buyer)
        {
            try
            {
                var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id).Products;
                string messsage;
                if (shoppingCart.Count != 0)
                {
                    var hasPayment = await _shoppingCartService.PaymentAsync(buyer, shoppingCart);
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
            catch (Exception)
            {
                throw;
            }
        }

        public void SelectProducts(Buyer buyer)
        {
            try
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the SelectProducts method. Message - {ex.Message}");
            }
            Clear(0);
        }

        public void ShowCart(Buyer buyer)
        {
            var positionNumber = 0;
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the ShowCart method. Message - {ex.Message}");
            }
            Clear(0);
        }

        public void AddProduct()
        {
            try
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the AddProduct method. Message - {ex.Message}");
            }
            Clear(0);
        }

        public void RemoveProduct()
        {
            var productId = 0;
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the RemoveProduct method. Message - {ex.Message}");
            }
            Clear(0);
        }

        public void UpdateProduct()
        {
            var productId = 0;
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception in the UpdateProduct method. Message - {ex.Message}");
            }
            Clear(0);
        }

        public bool SignOut(string login)
        {
            _logger.LogInformation($"User with login:{login} is logged out");
            return false;
        }

        private void SetCulture(string culture)
        {
            var specificCulture = CultureInfo.CreateSpecificCulture(culture);
            Thread.CurrentThread.CurrentCulture = specificCulture;
            Thread.CurrentThread.CurrentUICulture = specificCulture;
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