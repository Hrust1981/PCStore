using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IProductRepository _repository;
        private Dictionary<int, int> _quantityInStock;
        private int _oldQuantityValue;

        public ShoppingCartService(IProductRepository repository)
        {
            _repository = repository;
            _quantityInStock = new();
        }

        public void AddProduct(Buyer buyer, int productId)
        {
            if (productId > 0 && productId <= _repository.Count)
            {
                var selectedProduct = _repository.Get(productId);
                if (!_quantityInStock.Any(id => id.Key == productId))
                {
                    _quantityInStock.Add(selectedProduct.Id, selectedProduct.Quantity);
                }
                
                if (selectedProduct.Quantity > 0)
                {
                    var shoppingCart = buyer.ShoppingCart;
                    if (shoppingCart.Any(p => p.Id == productId))
                    {
                        shoppingCart.FirstOrDefault(p => p.Id == productId).Quantity++;
                    }
                    else
                    {
                        shoppingCart.Add(new ProductDTO(selectedProduct.Id, selectedProduct.Name,
                                                        selectedProduct.Price, 1));
                    }
                    selectedProduct.Quantity--;
                }
            }
        }

        public void UpdateQuantityProduct(Buyer buyer, int productId, int quantity)
        {
            if (!buyer.ShoppingCart.Any(p => p.Id == productId))
            {
                throw new Exception($"Product with ID = {productId} was not found");
            }
            var product = _repository.Get(productId);
            if (quantity > 0 &&
                _oldQuantityValue != quantity &&
                quantity <= product.Quantity + 1)
            {
                buyer.ShoppingCart.FirstOrDefault(p => p.Id == productId).Quantity = quantity;
                var quantityProduct = _quantityInStock.FirstOrDefault(x => x.Key == productId).Value;
                _repository.Update(new Product(product.Id, product.Name, product.Description,
                                               product.Price, quantityProduct - quantity));
                _oldQuantityValue = quantity;
            }
        }

        public void DeleteProduct(Buyer buyer, int productId)
        {
            if (!buyer.ShoppingCart.Any(p => p.Id == productId))
            {
                throw new Exception($"Product with ID = {productId} was not found");
            }
            var product = buyer.ShoppingCart.FirstOrDefault(p => p.Id == productId);
            var productFromDB = _repository.Get(productId);
            productFromDB.Quantity += product.Quantity;
            _repository.Update(productFromDB);
            buyer.ShoppingCart.Remove(product);
        }

    }
}
