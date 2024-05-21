using Core.Entities;

namespace Core.Data
{
    public class DB
    {
        public static List<User> _users = new List<User>()
        {
            new Seller("Seller1", "Seller1", "Seller1", "seller1@mailru"),
            new Seller("Seller2", "Seller2", "Seller2", "seller2@mailru"),
        };
    }
}
