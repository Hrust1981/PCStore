using Core.Entities;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        public User GetUserByLogin(string login);
    }
}
