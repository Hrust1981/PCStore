namespace Core.Entities
{
    public class Seller : User
    {
        private bool _isAuthenticated;

        public Seller(string name, string login, string password, string email) : base(name, login, password, email)
        {
            Name = name;
            Login = login;
            Password = password;
            Email = email;
        }

        public override string Name { get; set; } = string.Empty;
        public override string Login { get; set; } = string.Empty;
        public override string Password { get; set; } = string.Empty;
        public override string Email { get; set; } = string.Empty;
        public override bool IsAuthenticated { get => _isAuthenticated; set { _isAuthenticated = value; } }
    }
}
