using Core.Entities;

namespace Core.Data
{
    public interface IUserRepository
    {
        User Get(string login);
        void Add(User seller);
        void Update(User seller);
        void Delete(string login);
    }
}
