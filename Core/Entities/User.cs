namespace Core.Entities
{
    public class User : Entity
    {
        public User(string name, string login, string password, string email, Role role) : base(name)
        {
            //Id = DB.CounterUserId;
            Id = Guid.NewGuid();
            Name = name;
            Login = login;
            Password = password;
            Email = email;
            Role = role;
        }

        //public override int Id { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
