using Core;
using Core.Data;
using Core.Entities;
using Core.Repositories;

namespace TUI
{
    public class UI
    {
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

        public int Menu(User user)
        {
            string message = string.Empty;

            if (user.Role == Role.Buyer)
            {
                message = $"""
                        1.Выбрать товар
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
            int.TryParse(value, out int outValue);
            return outValue;
        }

        public void Payment()
        {

        }

        public void Goods()
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
