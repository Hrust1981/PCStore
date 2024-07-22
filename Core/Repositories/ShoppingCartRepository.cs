using Core.Data;
using Core.Entities;

namespace Core.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly List<ShoppingCart> _carts;
        public ShoppingCartRepository()
        {
            _carts = DB.shoppingCarts;
            QuantityInStock = new();
        }

        public Dictionary<Guid, int> QuantityInStock { get; set; }

        public ShoppingCart GetByUserId(Guid userId)
        {
            var cart = _carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                throw new Exception($"Shopping cart with ID:{userId} not found");
            }
            return cart;
        }
    }
}