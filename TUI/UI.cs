using Core;
using Core.Entities;
using Core.Enumerations;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Reflection;
using TUI.Properties;

namespace TUI
{
    public class UI
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAuthentication _authentication;
        private readonly IDiscountCardService _discountCardService;
        private readonly IStringLocalizer _stringLocalizer;
        private readonly LoggerOptions _setupOptions;
        private readonly ILogger<UI> _logger;

        public UI(IRepository<Product> repository,
                  IShoppingCartService shoppingCartService,
                  IShoppingCartRepository shoppingCartRepository,
                  IAuthentication authentication,
                  IDiscountCardService discountCardService,
                  IStringLocalizer stringLocalizer,
                  IOptionsMonitor<LoggerOptions> setupOptions,
                  ILogger<UI> logger)
        {
            _productRepository = repository;
            _shoppingCartService = shoppingCartService;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
            _authentication = authentication;
            _discountCardService = discountCardService;
            _stringLocalizer = stringLocalizer;
            _setupOptions = setupOptions.CurrentValue;
        }

        public void SelectCulture()
        {
            try
            {
                var displayMessage = _stringLocalizer["SelectLanguage"];
                var positionNumber = (Culture)GetEnteredNumericValue(displayMessage);

                if (positionNumber == Culture.English)
                {
                    SetCulture("en-US");
                }
                else if (positionNumber == Culture.Russian)
                {
                    SetCulture("ru-RU");
                }
                else
                {
                    throw new Exception($"""
                        Entered value: '{positionNumber}'.
                        Expected values: '{displayMessage}'
                        """);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(1500);
        }

        public User Authentication()
        {
            User? user = null;
            try
            {
                string outputMessage = _stringLocalizer["Login"];
                Display(outputMessage);
                string login = DataInput();

                outputMessage = _stringLocalizer["Password"];
                DisplayLine(outputMessage);
                string password = HiddenPasswordInput();

                user = _authentication.Authenticate(login, password);

                if (user.IsAuthenticated)
                {
                    outputMessage = _stringLocalizer["Hello"] + user.Name + ", " + _stringLocalizer["LoggedIn"];
                    _logger.LogInformation($"User with login:{user.Login} is authorized.");
                }
                else
                {
                    outputMessage = _stringLocalizer["NotLoggedIn"];
                    _logger.LogWarning($"User with login:{user.Login} is not authorized.");
                }

                DisplayLine(outputMessage);
                Clear(2000);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
                Clear(100);
            }
            return user;
        }

        public int Menu(User user)
        {
            var value = 0;
            try
            {
                string displayMessage = string.Empty;
                if (user.Role == Role.Admin)
                {
                    displayMessage = _stringLocalizer["MenuForAdmin"];
                }
                else if (user.Role == Role.Buyer)
                {
                    displayMessage = _stringLocalizer["MenuForBuyer"];
                }
                else if (user.Role == Role.Seller)
                {
                    displayMessage = _stringLocalizer["MenuForSeller"];
                }
                value = GetEnteredNumericValue(displayMessage);
                Clear(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
                Clear(100);
            }
            return value;
        }

        public void BuyCheerfulDiscountCard(Buyer buyer)
        {
            try
            {
                var displayMessage = string.Empty;
                var discountCards = buyer.DiscountCards;

                if (!discountCards.Any(dc => string.Equals(dc.Name, "CheerfulDiscountCard")))
                {
                    _discountCardService.AddCheerfulDiscountCard(buyer);
                    displayMessage = _stringLocalizer["CheerfulDiscountCardPurchased"];
                }
                else
                {
                    displayMessage = _stringLocalizer["CheerfulDiscountCardAlreadyPurchased"];
                }
                Display(displayMessage);
                Clear(2500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(100);
        }

        public void ShowPathForLoggingFile()
        {
            try
            {
                DisplayLine(_stringLocalizer["PathToLogFile"]);
                var path = _setupOptions.PathToLoggerFile;
                Display(path);
                Clear(2500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(100);
        }

        public void SettingsForDiscountCards()
        {
            try
            {
                var positionNumber = default(SettingValueDiscountCard);
                while (positionNumber != SettingValueDiscountCard.Exit)
                {
                    var displayMessage = _stringLocalizer["DiscountCardSettingsMenu"];
                    positionNumber = (SettingValueDiscountCard)GetEnteredNumericValue(displayMessage);

                    if (positionNumber == SettingValueDiscountCard.DateIssueQuantumDiscountCard)
                    {
                        Clear(100);
                        displayMessage = _stringLocalizer["SpecialDayForQuantumDiscountCard"];
                        DisplayLine(displayMessage);

                        var date = _discountCardService.SetDateIssueForQuantumDiscountCard();

                        Display(date.ToString());
                        _logger.LogInformation($"The day for issuing a quantum discount card has been determined - {date}.");
                    }
                    else if (positionNumber == SettingValueDiscountCard.ValidityPeriodQuantumDiscountCard)
                    {
                        Clear(100);
                        displayMessage = _stringLocalizer["ValidityPeriodQuantumDiscountCard"];

                        var numberDays = GetEnteredNumericValue(displayMessage);

                        _discountCardService.SetDayForIssueQuantumDiscountCard(numberDays);
                        _logger.LogInformation($"The validity period of the Quantum discount card has been changed to {numberDays} days.");
                    }
                    else if (positionNumber == SettingValueDiscountCard.WorkDatesCheerfulDiscountCard)
                    {
                        Clear(100);
                        displayMessage = _stringLocalizer["WorkDatesCheerfulDiscountCard"];
                        DisplayLine(displayMessage);

                        var date = _discountCardService.SetWorkDatesForCheerfulDiscountCard();

                        Display(date.ToString());
                        _logger.LogInformation($"{date}- date of issue of the Fun discount card.");
                    }
                    Clear(2500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(100);
        }

        private void Payment(Buyer buyer)
        {
            try
            {
                var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id).Products;
                string displayMesssage;
                if (shoppingCart.Count != 0)
                {
                    var hasPayment = _shoppingCartService.Payment(buyer, shoppingCart);
                    displayMesssage = hasPayment ? _stringLocalizer["SuccessfulPayment"] : _stringLocalizer["SuccessfulNotPayment"];
                    Display(displayMesssage);
                }
                else
                {
                    displayMesssage = _stringLocalizer["CartIsEmpty"];
                    Display(displayMesssage);
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
                while (productId != Constants.EXIT)
                {
                    var products = _productRepository.GetAll();
                    if (products.Count == 0)
                    {
                        _logger.LogWarning("There are no products in the database.");
                    }

                    var idCounter = 0;
                    foreach (var product in products)
                    {
                        DisplayLine(++idCounter + product.ToString());
                    }

                    var displayMessage = _stringLocalizer["Exit"];
                    DisplayLine(displayMessage);

                    displayMessage = _stringLocalizer["AddItemToCart"];
                    productId = GetEnteredNumericValue(displayMessage);

                    if (productId > 0)
                    {
                        _shoppingCartService.AddProduct(buyer, products[productId - 1]);
                    }
                    Clear(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(0);
        }

        public void ShowCart(Buyer buyer)
        {
            var positionNumber = default(ShowCartMenu);
            try
            {
                while (positionNumber != ShowCartMenu.Exit)
                {
                    var products = _shoppingCartRepository.GetByUserId(buyer.Id).Products;
                    var idCounter = 0;
                    foreach (var product in products)
                    {
                        DisplayLine(++idCounter + product.ToString());
                    }

                    string? displayMessage;
                    if (products.Count != 0)
                    {
                        var totalAmount = _shoppingCartService.CalculateTotalAmount(buyer);

                        displayMessage = _stringLocalizer["AmountToPay"] + totalAmount + _stringLocalizer["RUB"];
                        DisplayLine(displayMessage);
                    }

                    displayMessage = _stringLocalizer["MenuShowCart"];
                    positionNumber = (ShowCartMenu)GetEnteredNumericValue(displayMessage);

                    displayMessage = _stringLocalizer["EnterProductID"];
                    switch (positionNumber)
                    {
                        case ShowCartMenu.PayGoods:
                            Payment(buyer);
                            break;
                        case ShowCartMenu.ChangeProductQuantity:
                            {
                                var productId = GetEnteredNumericValue(displayMessage);

                                displayMessage = _stringLocalizer["EnterQuantity"];
                                var quantity = GetEnteredNumericValue(displayMessage);

                                _shoppingCartService.UpdateQuantityProduct(buyer, products[productId - 1], quantity);
                                break;
                            }
                        case ShowCartMenu.RemoveItemFromCart:
                            {
                                var productId = GetEnteredNumericValue(displayMessage);
                                _shoppingCartService.DeleteProduct(buyer, products[productId - 1]);
                                break;
                            }
                    }
                    Clear(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(0);
        }

        public void AddProduct()
        {
            try
            {
                var displayMessage = _stringLocalizer["EnterProductName"];
                var name = GetEnteredStringValue(displayMessage);

                displayMessage = _stringLocalizer["EnterProductDescription"];
                var description = GetEnteredStringValue(displayMessage);

                displayMessage = _stringLocalizer["EnterPriceProduct"];
                var price = GetEnteredNumericValue(displayMessage);

                displayMessage = _stringLocalizer["EnterQuantityProduct"];
                var quantity = GetEnteredNumericValue(displayMessage);

                _productRepository.Add(new Product(name, description, price, quantity));
                _logger.LogInformation($"The new product named '{name}' has been added to the product repository.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(0);
        }

        public void RemoveProduct()
        {
            var productId = 0;
            try
            {
                while (productId != Constants.EXIT)
                {
                    var products = _productRepository.GetAll();
                    if (products.Count == 0)
                    {
                        _logger.LogWarning("There are no products in the database.");
                    }

                    var idCounter = 0;
                    foreach (var product in products)
                    {
                        DisplayLine(++idCounter + product.ToString());
                    }

                    var displayMessage = _stringLocalizer["Exit"];
                    DisplayLine(displayMessage);

                    displayMessage = _stringLocalizer["ToDeleteEnterProductID"];
                    productId = GetEnteredNumericValue(displayMessage);

                    if (productId > 0)
                    {
                        var deletetableProduct = products[productId - 1];
                        if (deletetableProduct != null)
                        {
                            _productRepository.Delete(deletetableProduct.Id);
                            _logger.LogInformation($"Product with ID:{productId} has been removed from the product repository.");
                        }
                        else
                        {
                            _logger.LogWarning($"Product with ID:{productId} not found.");
                            throw new Exception($"Product with ID:{productId} is not found.");
                        }
                    }
                    Clear(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
            }
            Clear(0);
        }

        public void UpdateProduct()
        {
            var productId = 0;
            try
            {
                while (productId != Constants.EXIT)
                {
                    var products = _productRepository.GetAll();
                    if (products.Count == 0)
                    {
                        _logger.LogWarning("There are no products in the database.");
                    }

                    var idCounter = 0;
                    foreach (var product in products)
                    {
                        DisplayLine(++idCounter + product.ToString());
                    }

                    var displayMessage = _stringLocalizer["Exit"];
                    DisplayLine(displayMessage);

                    displayMessage = _stringLocalizer["ToEditEnterProductID"];
                    productId = GetEnteredNumericValue(displayMessage);

                    if (productId > 0)
                    {
                        var updatableProduct = products[productId - 1];
                        if (updatableProduct != null)
                        {
                            displayMessage = _stringLocalizer["EnterProductName"];
                            var enteredName = GetEnteredStringValue(displayMessage);
                            var name = string.IsNullOrEmpty(enteredName) ? updatableProduct.Name : enteredName;

                            displayMessage = _stringLocalizer["EnterProductDescription"];
                            var enteredDescription = GetEnteredStringValue(displayMessage);
                            var description = string.IsNullOrEmpty(enteredDescription) ? updatableProduct.Description : enteredDescription;

                            displayMessage = _stringLocalizer["EnterPriceProduct"];
                            var enteredPrice = GetEnteredNumericValue(displayMessage);
                            var price = enteredPrice > 0 ? enteredPrice : updatableProduct.Price;

                            displayMessage = _stringLocalizer["EnterQuantityProduct"];
                            var enteredQuantity = GetEnteredNumericValue(displayMessage);
                            var quantity = enteredQuantity > 0 ? enteredQuantity : updatableProduct.Quantity;

                            _productRepository.Update(new Product(updatableProduct.Id, name, description, price, quantity));
                            _logger.LogInformation($"Product with ID:{updatableProduct.Id} updated.");
                        }
                        else
                        {
                            _logger.LogWarning($"Product with ID:{productId} not found.");
                            throw new Exception($"Product with ID:{productId} is not found.");
                        }
                    }
                    Clear(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}. This exception occurred in the {ex.TargetSite} method.");
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