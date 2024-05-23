namespace TUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            UI ui = new UI();
            while (true)
            {
                var user = ui.Authentication();
                bool isAuthenticated = user.IsAuthenticated;
                while (isAuthenticated)
                {
                    var menuItem = ui.Menu(user);
                    if (user.Role == Core.Role.Buyer)
                    {
                        switch (menuItem)
                        {
                            case 1:
                                break;
                            case 2:
                                ui.Payment();
                                break;
                            case 3:
                                break;
                            case 4:
                                break;
                            default:
                                break;

                        }
                    }
                    else if (user.Role == Core.Role.Seller)
                    {
                        switch (menuItem)
                        {
                            case 1:
                                break;
                            case 2:
                                break;
                            case 3:
                                break;
                            default:
                                break;

                        }
                    }
                }
            }
        }
    }
}
