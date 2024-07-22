using Core.Entities;
using Core.Repositories;

namespace Core
{
    public class Authentication : IAuthentication
    {
        private readonly IUserRepository _userRepository;

        public Authentication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string login, string password)
        {
            var user = _userRepository.GetUserByLogin(login);
            user.IsAuthenticated = string.Equals(user.Password, password, StringComparison.InvariantCulture);
            return user;
        }
    }
}
