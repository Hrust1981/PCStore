using Core.Entities;

namespace Core.Repositories
{
    public class SellerRepository : IUserRepository
    {
        private List<User> _sellers;

        public SellerRepository(List<User> sellers)
        {
            _sellers = sellers;
        }

        public User Get(string login)
        {
            var seller = _sellers.FirstOrDefault(s => string.Equals(s.Login, login));
            if (seller == null)
            {
                throw new Exception($"Seller with login '{login}' is not found");
            }
            return seller;
        }

        public void Add(User seller)
        {
            if (_sellers.Any(s => string.Equals(s.Login, seller.Login)))
            {
                throw new Exception($"Seller with login '{seller.Login}' already exists");
            }
            _sellers.Add(new Seller(seller.Name, seller.Login, seller.Password, seller.Email));
        }

        // ToDo: Use automapper
        public void Update(User seller)
        {
            var foundSeller = Get(seller.Login);
            foundSeller.Name = seller.Name;
            foundSeller.Password = seller.Password;
            foundSeller.Email = seller.Email;
        }

        public void Delete(string login)
        {
            var seller = _sellers.FirstOrDefault(s => string.Equals(s.Login, login));
            if (seller == null)
            {
                return;
            }
            _sellers.Remove(seller);
        }
    }
}
