using Core.Entities;

namespace Core.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>
    {
        public ShoppingCartRepository(List<ShoppingCart> entities) : base(entities)
        {
        }
    }
}
