using AutoMapper;
using Core.Entities;

namespace Core.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public ProductRepository(List<Product> products)
        {
            _products = products;
        }

        public int Count { get => _products.Count; }

        public void Add(Product product)
        {
            if (_products.Any(p => p.Id == product.Id))
            {
                throw new Exception($"Product with ID {product.Id} already exist");
            }
            _products.Add(new Product(product.Name, product.Description, product.Price, product.Quantity));
        }

        public Product Get(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new Exception($"Product with {id} is not found");
            }
            return product;
        }

        public List<Product> GetAll()
        {
            return _products.Where(p => p.Quantity > 0).ToList();
        }

        public void Update(Product product)
        {
            var updateProduct = Get(product.Id);
            if (updateProduct == null)
            {
                throw new Exception($"Product with ID {product.Id} was not found");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Product, Product>());
            var mapper = config.CreateMapper();
            mapper.Map<Product, Product>(product, updateProduct);
        }

        public void Delete(int id)
        {
            var product = Get(id);
            if (product == null)
            {
                return;
            }
            _products.Remove(product);
        }
    }
}
