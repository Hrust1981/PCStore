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
                    ui.Menu(user);
                }
            }
        }
    }
}
