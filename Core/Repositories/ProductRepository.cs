using Core.Data;
using Core.Entities;

namespace Core.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        private readonly List<Product> _products;

        public ProductRepository(List<Product> products) : base(products)
        {
            _products = products;
        }

        public int Count { get => _products.Count; }
    }
}
