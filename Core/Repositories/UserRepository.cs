using AutoMapper;
using Core.Entities;

namespace Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository(List<User> users)
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
            var user = Get(login);
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
                throw new Exception($"{user.Role} with login '{login}' was not found");
            }
            return user;
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public void Update(User user)
        {
            var updateUser = Get(user.Login);
            if (updateUser == null)
            {
                throw new Exception($"User with login {user.Login} was not found");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, User>());
            var mapper = config.CreateMapper();            
            mapper.Map<User, User>(user, updateUser);
        }
    }
}
