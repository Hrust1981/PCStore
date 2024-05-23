namespace Core.Entities
{
    public class Buyer : User
    {
        private bool _isAuthenticated;
        private static List<Product> _shoppingCart;

        public Buyer(string name, string login, string password, string email, Role role, DiscountCard discountCard)
            : base(name, login, password, email, role)
        {
        }

        public bool IsAuthenticated { get => _isAuthenticated; set { _isAuthenticated = value; } }
        public DiscountCard DiscountCard { get; set; }
        public static List<Product> ShoppingCart { get => _shoppingCart; set { _shoppingCart = value; } }
    }
}
