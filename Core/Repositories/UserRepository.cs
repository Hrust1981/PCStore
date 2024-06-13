using Core.Entities;

namespace Core.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly List<User> _users;
        public UserRepository(List<User> users) : base(users)
        {
            _users = users;
        }

        public User Get(string login)
        {
            var user = _users.FirstOrDefault(s => string.Equals(s.Login, login));
            if (user == null)
            {
                throw new Exception($"User with login:{login} not found");
            }
            return user;
        }
    }
}
