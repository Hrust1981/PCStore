using Core;
using Core.Data;
using Core.Entities;
using Core.Repositories;

namespace TUI
{
    public class UI
    {
        private readonly ProductRepository _repository;

        public UI(ProductRepository repository)
        {
            _repository = repository;
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
                message = $"""
                        1.Каталог товаров
                        2.Оплата
                        3.Корзина
                        4.Разлогиниться{Environment.NewLine}
                        """;
            }
            else if (user.Role == Role.Seller)
            {
                message = $"""
                        1.Добавить товар
                        2.Удалить товар
                        3.Разлогиниться{Environment.NewLine}
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

        public void ShowProducts()
        {
            var enteredValue = string.Empty;
            while (!string.Equals(enteredValue.ToLower(), "q"))
            {
                var products = _repository.GetAll();
                foreach (var product in products)
                {
                    Console.WriteLine(product);
                }
                Display("Выбери товар: ");
                enteredValue = DataInput();
                int.TryParse(enteredValue, out int valueId);
                if (valueId > 0 && valueId <= _repository.Count)
                {
                    var selectedProduct = _repository.Get(valueId);
                    selectedProduct.Quantity--;
                    _repository.Update(selectedProduct); 
                }
                Clear(0);
            }
            Clear(0);
        }

        public void ShowCart()
        {

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
