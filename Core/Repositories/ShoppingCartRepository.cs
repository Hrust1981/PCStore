using AutoMapper;
using Core.Entities;

namespace Core.Repositories
{
    public class ShoppingCartRepository : ProductRepository
    {
        public ShoppingCartRepository(List<Product> products) : base(products)
        {
        }
    }
}
