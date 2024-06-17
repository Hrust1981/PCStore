using Core.Entities;
using Core.Repositories;

namespace Core
{
    public class Authentication : IAuthentication
    {
        private UserRepository _userRepository;

        public Authentication(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string login, string password)
        {
            var user = _userRepository.Get(login);
            user.IsAuthenticated = string.Equals(user.Password, password, StringComparison.InvariantCulture);
            return user;
        }
    }
}
