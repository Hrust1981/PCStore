using Core.Data;
using Core.Entities;

namespace Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;
        public UserRepository()
        {
            _users = DB.users;
        }

        public User GetUserByLogin(string login)
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
