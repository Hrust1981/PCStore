using Core.Repositories;
using Core.Entities;

namespace Core.Repositories
{
    public abstract class UserRepository : IUserRepository
    {
        private List<User> _users;

        protected UserRepository(List<User> users)
        {
            _users = users;
        }

        public void Add(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(string login)
        {
            throw new NotImplementedException();
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

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
