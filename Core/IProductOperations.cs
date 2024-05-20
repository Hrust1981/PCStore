using Core.Entities;

namespace Core
{
    public interface IProductOperations
    {
        void AddProduct(Product product);

        void DeleteProduct(Product product);
    }
}
