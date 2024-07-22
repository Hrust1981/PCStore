using Core.Data;
using Core.Entities;

namespace Core.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        public ProductRepository() : base(DB.products)
        {
        }
    }
}
