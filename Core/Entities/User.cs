namespace Core.Entities
{
    public class User
    {
        public User(string name, string login, string password, string email, Role role)
        {
            Name = name;
            Login = login;
            Password = password;
            Email = email;
            Role = role;
        }

        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
