using Core;
using Core.Data;

namespace GUI
{
    public class UI
    {
        public bool Authentication()
        {
            string message = $"Авторизуйтесь{Environment.NewLine}Логин: ";
            Display(message);
            string login = Console.ReadLine();
            Display("Пароль: ");
            string password = HiddenPasswordInput();

            Authentication authentication
                = new Authentication(new SellerRepository());
            
            var user = authentication.Authenticate(login, password);
            message = user.IsAuthenticated ?
                $"{Environment.NewLine}Здравствуйте {user.Name}, Вы авторизованы!{Environment.NewLine}" :
                $"{Environment.NewLine}Вы не авторизовались!{Environment.NewLine}";

            Display(message);
            Clear(2000);
            return user.IsAuthenticated;
        }

        public void Payment()
        {

        }

        public int Menu()
        {
            string message = $"1.Оплата{Environment.NewLine}2.Возврат{Environment.NewLine}3.Товары{Environment.NewLine}4.Разлогиниться{Environment.NewLine}";
            Display(message);
            string value = Console.ReadLine();
            if (!value.Any(x => x.Equals("1") ||
                                x.Equals("2") ||
                                x.Equals("3") ||
                                x.Equals("4")))
            {
                Clear(0);
                Menu();
            }
            int.TryParse(value, out int outValue);
            return outValue;
        }

        public void Refund()
        {

        }

        public void Goods()
        {

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
