using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IProductRepository _repository;
        private int _quantityInStock;
        private int _oldQuantityValue;

        public ShoppingCartService(IProductRepository repository)
        {
            _repository = repository;
        }

        public void AddProduct(Buyer buyer, int id)
        {
            if (id > 0 && id <= _repository.Count)
            {
                var selectedProduct = _repository.Get(id);
                _quantityInStock = selectedProduct.Quantity;
                if (selectedProduct.Quantity > 0)
                {
                    var shoppingCart = buyer.ShoppingCart;
                    if (shoppingCart.Any(p => p.Id == id))
                    {
                        shoppingCart.FirstOrDefault(p => p.Id == id).Quantity++;
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

        public void UpdateQuantityProduct(Buyer buyer, int id, int quantity)
        {
            var product = _repository.Get(id);
            if (quantity > 0 &&
                quantity <= product.Quantity + 1 &&
                _oldQuantityValue != quantity)
            {
                buyer.ShoppingCart.FirstOrDefault(p => p.Id == id).Quantity = quantity;
                _repository.Update(new Product(product.Id, product.Name, product.Description,
                                               product.Price, _quantityInStock - quantity));
                _oldQuantityValue = quantity;
            }
        }

        public void DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

    }
}
