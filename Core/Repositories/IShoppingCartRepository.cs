using Core.Entities;

namespace Core.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetByUserId(Guid id);
        List<ShoppingCart> GetAll();
    }
}
