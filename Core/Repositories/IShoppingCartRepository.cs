using Core.Entities;

namespace Core.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetByUserId(Guid id);
        List<ShoppingCart> GetAll();
        public Dictionary<Guid, int> QuantityInStock { get; set; }
    }
}
