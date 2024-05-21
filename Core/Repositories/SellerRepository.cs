using Core.Entities;

namespace Core.Repositories
{
    public class SellerRepository : UserRepository
    {
        public SellerRepository(List<User> sellers) : base(sellers)
        {
        }
    }
}
