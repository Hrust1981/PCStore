namespace Core.Entities
{
    public class Buyer : User
    {
        private bool _isAuthenticated;

        public Buyer(string name, string login, string password, string email, Role role, DiscountCard discountCard)
            : base(name, login, password, email, role)
        {
        }

        public bool IsAuthenticated { get => _isAuthenticated; set { _isAuthenticated = value; } }
        public DiscountCard DiscountCard { get; set; }
    }
}
