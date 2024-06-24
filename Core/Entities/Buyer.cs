namespace Core.Entities
{
    public class Buyer : User
    {
        private bool _isAuthenticated;
        //private ShoppingCart _shoppingCart;
        private List<DiscountCard> _discountCards;
        private int _totalPurchaseAmount;

        public Buyer(string name, string login, string password, string email, Role role)
            : base(name, login, password, email, role)
        {
            //_shoppingCart = ShoppingCart;
            _discountCards = new List<DiscountCard>();
        }

        public bool IsAuthenticated { get => _isAuthenticated; set { _isAuthenticated = value; } }
        public List<DiscountCard> DiscountCards { get => _discountCards; set { _discountCards = value; } }
        //public ShoppingCart ShoppingCart { get => _shoppingCart; set { _shoppingCart = value; } }
        public int TotalPurchaseAmount { get => _totalPurchaseAmount; set { _totalPurchaseAmount = value; } }
    }
}
