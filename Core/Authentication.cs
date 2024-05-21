using Core.Entities;
using Core.Repositories;

namespace Core
{
    public class Authentication : IAuthentication
    {
        private IUserRepository _userRepository;

        public Authentication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string login, string password)
        {
            var user = _userRepository.Get(login);
            user.IsAuthenticated = string.Equals(user.Password, password) ? user.IsAuthenticated = true : false;
            return user;
        }
    }
}
