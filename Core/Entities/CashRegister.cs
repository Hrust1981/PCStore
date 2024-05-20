using Core.Data;

namespace Core.Entities
{
    public class CashRegister : IProductOperations, IPayment
    {
        private ProductRepository _productsRepository;

        public CashRegister(ProductRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public void AddProduct(Product product)
        {
            _productsRepository.products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _productsRepository.products.Remove(product);
        }

        public void Payment(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
