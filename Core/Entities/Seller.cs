namespace Core.Entities
{
    public class Seller : User
    {
        public Seller(string name, string login, string password, string email, Role role)
            : base(name, login, password, email, role)
        {
        }
        public bool IsAuthenticated { get; set; }
    }
}
