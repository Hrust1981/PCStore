using Core.Entities;
using Core.Repositories;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IDiscountCardService _discountCardService;
        private readonly ILogger<ShoppingCartService> _logger;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository,
                                   IRepository<Product> productRepository,
                                   IDiscountCardService discountCardService,
                                   ILogger<ShoppingCartService> logger)
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _discountCardService = discountCardService;
            _logger = logger;
        }

        public void AddProduct(Buyer buyer, Product product)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id);

            if (!_shoppingCartRepository.QuantityInStock.Any(id => id.Key == product.Id))
            {
                _shoppingCartRepository.QuantityInStock.Add(product.Id, product.Quantity);
            }

            if (product.Quantity > 0)
            {
                var products = shoppingCart.Products;
                if (products.Any(p => p.Id == product.Id))
                {
                    products.FirstOrDefault(p => p.Id == product.Id)!.Quantity++;
                }
                else
                {
                    products.Add(new Product(product.Id, product.Name,
                                             product.Description, product.Price, 1));
                }
                product.Quantity--;
            }
            _logger.LogInformation($"Product with ID:{product.Id} has been added to shopping cart");
        }

        public void UpdateQuantityProduct(Buyer buyer, Product updatableProduct, int quantity)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id);
            var products = shoppingCart.Products;
            if (products == null)
            {
                _logger.LogWarning($"There are no items in the cart of the user with ID:{buyer.Id}");
                return;
            }

            var quantityProduct = _shoppingCartRepository.QuantityInStock.FirstOrDefault(x => x.Key == updatableProduct.Id).Value;
            var product = products.FirstOrDefault(p => p.Id == updatableProduct.Id);
            var oldQuantityValue = product?.Quantity;

            if (quantity > 0 && quantityProduct - quantity >= 0)
            {
                product!.Quantity = quantity;
                _productRepository.Update(new Product(updatableProduct.Id, updatableProduct.Name, updatableProduct.Description,
                                                      updatableProduct.Price, quantityProduct - quantity));
            }
            _logger.LogInformation($"The quantity of product with ID:{updatableProduct.Id} has been updated from {oldQuantityValue} to {quantity} pieces to the shopping cart");
        }

        public void DeleteProduct(Buyer buyer, Product product)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id);
            var productFromDB = _productRepository.GetAll().FirstOrDefault(p => p.Id == product.Id);

            if (productFromDB != null && product != null)
            {
                productFromDB.Quantity += product.Quantity;
                _productRepository.Update(productFromDB);
                shoppingCart.Products.Remove(product);
            }
            _logger.LogInformation($"Product with ID:{product.Id} has been removed from the shopping cart");
        }

        public int CalculateTotalAmount(Buyer buyer)
        {
            var discountCard = buyer.DiscountCards?.MaxBy(d => d.Discount);
            var totalAmount = _shoppingCartRepository.GetByUserId(buyer.Id).Products.Sum(s => s.Price * s.Quantity);
            var totalAmountWithDiscount = discountCard == null ?
                0 :
                totalAmount - totalAmount * discountCard.Discount / 100;

            return discountCard == null ? totalAmount : totalAmountWithDiscount;
        }

        public bool Payment(Buyer buyer, List<Product> shoppingCart)
        {
            int totalAmount = 0;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    totalAmount = CalculateTotalAmount(buyer);
                    buyer.TotalPurchaseAmount += totalAmount;
                    _discountCardService.AddDiscountCard(buyer);
                    shoppingCart.Clear();
                    _shoppingCartRepository.QuantityInStock.Clear();
                    scope.Complete();
                    _logger.LogInformation($"The goods were paid for in the amount of {totalAmount} RUB by the user {buyer.Login}");
                    return true;
                }
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogWarning($"Payment in the amount of {totalAmount} RUB did not go through. Message {ex.Message}");
                return false;
            }
        }
    }
}
