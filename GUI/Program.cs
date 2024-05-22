namespace GUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                UI ui = new UI();
                var user = ui.Authentication();
                while (true)
                {
                    bool isAuthenticated = user.IsAuthenticated;
                    if (isAuthenticated)
                    {
                        ui.Menu(user);
                    }
                }
            }
        }
    }
}
