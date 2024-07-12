namespace Core.Entities
{
    public class User : Entity
    {
        private User() { }

        public User(string name, string login, string password, string email, Role role)
        {
            Id = Guid.NewGuid();
            Name = name;
            Login = login;
            Password = password;
            Email = email;
            Role = role;
        }

        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public Role Role { get; set; }
        public bool IsAuthenticated { get; set; }
        public static User Empty { get => new User(); }
    }
}
