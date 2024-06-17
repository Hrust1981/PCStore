using Core.Entities;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ShoppingCartService> _logger;
        private Dictionary<int, int> _quantityInStock;

        public Dictionary<int, int> GetQuantityInStock { get { return _quantityInStock; } set { _quantityInStock = value; } }

        public ShoppingCartService(ProductRepository repository, ILogger<ShoppingCartService> logger)
        {
            _productRepository = repository;
            _logger = logger;
            _quantityInStock = new();
        }

        public void AddProduct(Buyer buyer, int productId)
        {
            if (productId > 0 && productId <= _productRepository.Count)
            {
                var selectedProduct = _productRepository.Get(productId);
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
                _logger.LogInformation($"Product with ID:{productId} has been added to shopping cart");
            }
        }

        public void UpdateQuantityProduct(Buyer buyer, int productId, int quantity)
        {
            var shoppingCart = buyer.ShoppingCart;
            if (!shoppingCart.Any(p => p.Id == productId))
            {
                throw new Exception($"Product with ID:{productId} was not found");
            }
            var product = _productRepository.Get(productId);
            var quantityProduct = _quantityInStock.FirstOrDefault(x => x.Key == productId).Value;
            var oldQuantityValue = shoppingCart.FirstOrDefault(p => p.Id == productId).Quantity;
            if (quantity > 0 && quantityProduct - quantity >= 0)
            {
                shoppingCart.FirstOrDefault(p => p.Id == productId).Quantity = quantity;
                _productRepository.Update(new Product(product.Id, product.Name, product.Description,
                                               product.Price, quantityProduct - quantity));
            }
            _logger.LogInformation($"The quantity of product with ID:{productId} has been updated from {oldQuantityValue} to {quantity} pieces to the shopping cart");
        }

        public void DeleteProduct(Buyer buyer, int productId)
        {
            if (!buyer.ShoppingCart.Any(p => p.Id == productId))
            {
                throw new Exception($"Product with ID:{productId} was not found");
            }
            var product = buyer.ShoppingCart.FirstOrDefault(p => p.Id == productId);
            var productFromDB = _productRepository.Get(productId);
            productFromDB.Quantity += product.Quantity;
            _productRepository.Update(productFromDB);
            buyer.ShoppingCart.Remove(product);
            _logger.LogInformation($"Product with ID:{productId} has been removed from the shopping cart");
        }

        public int CalculateTotalAmount(Buyer buyer, out int totalAmountWithDiscount)
        {
            var discountCard = buyer.DiscountCards?.OrderByDescending(d => d.Discount).FirstOrDefault();
            var totalAmount = buyer.ShoppingCart.Sum(s => s.Price * s.Quantity);

            totalAmountWithDiscount = discountCard == null ?
                0 :
                totalAmountWithDiscount = totalAmount - totalAmount * discountCard.Discount / 100;

            return totalAmount;
        }

    }
}
