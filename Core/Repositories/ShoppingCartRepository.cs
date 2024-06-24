using Core.Entities;

namespace Core.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly List<ShoppingCart> _carts;
        public ShoppingCartRepository(List<ShoppingCart> carts)
        {
            _carts = carts;
        }

        public ShoppingCart GetByUserId(Guid id)
        {
            var cart = _carts.FirstOrDefault(c => c.UserId == id);
            if (cart == null)
            {
                throw new Exception($"Shopping cart with ID:{id} not found");
            }
            return cart;
        }

        public List<ShoppingCart> GetAll()
        {
            return _carts;
        }
    }
}