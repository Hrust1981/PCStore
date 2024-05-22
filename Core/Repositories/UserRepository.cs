using AutoMapper;
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
                throw new Exception($"{user.Role} with login '{user.Login}' already exists");
            }
            _users.Add(new User(user.Name, user.Login, user.Password, user.Email, user.Role));
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
            var user = _users.FirstOrDefault(s => string.Equals(s.Login, login));
            if (user == null)
            {
                throw new Exception($"{user.Role} with login '{login}' is not found");
            }
            return user;
        }

        public void Update(User user)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, User>());
            var mapper = config.CreateMapper();
            var foundUser = Get(user.Login);
            mapper.Map<User, User>(user, foundUser);
        }
    }
}
