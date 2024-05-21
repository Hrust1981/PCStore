using Core.Entities;

namespace Core.Repositories
{
    public abstract class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        protected UserRepository(List<User> users)
        {
            _users = users;
        }

        public void Add(User user)
        {
            if (_users.Any(s => string.Equals(s.Login, user.Login)))
            {
                throw new Exception($"Seller with login '{user.Login}' already exists");
            }
            _users.Add(new Seller(user.Name, user.Login, user.Password, user.Email));
        }

        public void Delete(string login)
        {
            var user = _users.FirstOrDefault(s => string.Equals(s.Login, login));
            if (user == null)
            {
                return;
            }
            _users.Remove(user);
        }

        public User Get(string login)
        {
            var users = _users.FirstOrDefault(s => string.Equals(s.Login, login));
            if (users == null)
            {
                throw new Exception($"Seller with login '{login}' is not found");
            }
            return users;
        }

        // ToDo: Use automapper
        public void Update(User user)
        {
            var foundSeller = Get(user.Login);
            foundSeller.Name = user.Name;
            foundSeller.Password = user.Password;
            foundSeller.Email = user.Email;
        }
    }
}
