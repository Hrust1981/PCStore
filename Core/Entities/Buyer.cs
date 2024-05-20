namespace Core.Entities
{
    public class Buyer : User
    {
        private bool _isAuthenticated;

        public Buyer(string name, string login, string password, string email, DiscountCard discountCard)
            : base(name, login, password, email)
        {
            Name = name;
            Login = login;
            Password = password;
            Email = email;
            DiscountCard = discountCard;
        }

        public override string Name { get; set; } = string.Empty;
        public override string Login { get; set; } = string.Empty;
        public override string Password { get; set; } = string.Empty;
        public override string Email { get; set; } = string.Empty;
        public override bool IsAuthenticated { get => _isAuthenticated; set { _isAuthenticated = value; } }
        public DiscountCard DiscountCard { get; set; }
    }
}
