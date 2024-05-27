using Core.Entities;

namespace Core.Services
{
    public interface IShoppingCartService
    {
        void Add(ProductDTO product, Buyer buyer, int valueId);
        void Update(ProductDTO product);
        void Delete(int id);
    }
}
