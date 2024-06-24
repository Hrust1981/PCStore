using Core.Entities;

namespace Core.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>
    {
        private readonly List<ShoppingCart> _products;
        public ShoppingCartRepository(List<ShoppingCart> products) : base(products)
        {
            _products = products;
        }
    }
}