namespace Core.Entities
{
    public class Buyer : User
    {
        private bool _isAuthenticated;
        private static List<ProductDTO>? _shoppingCart;

        public Buyer(string name, string login, string password, string email, Role role)
            : base(name, login, password, email, role)
        {
            _shoppingCart = new List<ProductDTO>();
        }

        public bool IsAuthenticated { get => _isAuthenticated; set { _isAuthenticated = value; } }
        public DiscountCard DiscountCard { get; set; }
        public List<ProductDTO> ShoppingCart { get => _shoppingCart; set { _shoppingCart = value; } }
    }
}
