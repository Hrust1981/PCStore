using Core.Enumerations;

namespace Core.Entities
{
    public class Admin : User
    {
        public Admin(string name, string login, string password, string email, Role role)
            : base(name, login, password, email, role)
        {
        }

        public bool IsAuthenticated { get; set; }
    }
}
