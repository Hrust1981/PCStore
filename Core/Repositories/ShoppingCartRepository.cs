using Core.Entities;

namespace Core.Repositories
{
    public class ShoppingCartRepository : Repository<ProductDTO>
    {
        private readonly List<ProductDTO> _products;
        public ShoppingCartRepository(List<ProductDTO> products) : base(products)
        {
            _products = products;
        }
    }
}
