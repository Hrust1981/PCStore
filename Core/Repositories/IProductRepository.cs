using Core.Entities;

namespace Core.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        List<Product> GetAll();
        Product Get(Guid id);
        void Update(Product product);
        void Delete(Guid id);
    }
}
