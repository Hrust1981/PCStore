using Core.Entities;

namespace Core.Services
{
    public interface IService
    {
        void Add(ProductDTO product);
        void Update(ProductDTO product);
        void Delete(int id);
    }
}
