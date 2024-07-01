using Core.Entities;

namespace Core.Services
{
    public interface IShoppingCartService
    {
        void AddProduct(Buyer buyer, Product product);
        void UpdateQuantityProduct(Buyer buyer, Product product, int quantity);
        void DeleteProduct(Buyer buyer, Product product);
        int CalculateTotalAmount(Buyer buyer);
        public bool Payment(Buyer buyer, List<Product> shoppingCart);
    }
}
