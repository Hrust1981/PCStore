using Core.Entities;

namespace Core
{
    public interface IAuthentication
    {
        User Authenticate(string login, string password);
    }
}
