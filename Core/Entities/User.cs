namespace Core.Entities
{
    public abstract class User
    {
        protected User(string name, string login, string password, string email)
        {
            Name = name;
            Login = login;
            Password = password;
            Email = email;
        }

        public abstract string Name { get; set; }
        public abstract string Login { get; set; }
        public abstract string Password { get; set; }
        public abstract string Email { get; set; }
        public abstract bool IsAuthenticated { get; set; }
    }
}
