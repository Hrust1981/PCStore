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

        public void Payment(Buyer buyer)
        {
            while (true)
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
                break;
            }
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
                var valueId = NewMethod("Выбери товар в корзину: ", out enteredValue);
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
                if (products.Any())
                {
                    var sum = buyer.ShoppingCart.Sum(x => x.Price * x.Quantity);
                    message = $"\t\tСумма к оплате: {sum} рублей";
                    DisplayLine(message);
                }
                message = $"""
                         1.Оплатить товар
                         2.Изменить количество в каждой позиции
                         3.Удалить товар из корзины
                         4.Для выхода 'q'

                         Выбери действие: 
                         """;
                var positionNumber = NewMethod(message, out enteredValue);
                if (positionNumber == 1)
                {
                    Payment(buyer);
                }
                else if (positionNumber == 2)
                {
                    var productId = NewMethod("Введи ID товара: ", out string unusedInputValue);
                    var quantity = NewMethod("Введи количество: ", out unusedInputValue);
                    _shoppingCartService.UpdateQuantityProduct(buyer, productId, quantity);
                }
                else if (positionNumber == 3)
                {
                    var productId = NewMethod("Введи ID товара: ", out string unusedInputValue);
                    _shoppingCartService.DeleteProduct(buyer, productId);
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

        private int NewMethod(string message, out string enteredValue)
        {
            Display(message);
            enteredValue = DataInput();
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
