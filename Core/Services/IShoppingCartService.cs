using Core.Entities;

namespace Core.Services
{
    public interface IShoppingCartService
    {
        public Dictionary<Guid, int> GetQuantityInStock { get;  set; }
        void AddProduct(Buyer buyer, Guid productId);
        void UpdateQuantityProduct(Buyer buyer, Guid productId, int quantity);
        void DeleteProduct(Buyer buyer, Guid productId);
        int CalculateTotalAmount(Buyer buyer, out int totalAmountWithDiscount);
    }
}
