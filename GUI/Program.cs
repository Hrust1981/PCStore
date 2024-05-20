namespace GUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                UI ui = new UI();
                bool isAuthenticated = ui.Authentication();
                if (isAuthenticated)
                {
                    ui.Menu();
                }
            }
        }
    }
}
