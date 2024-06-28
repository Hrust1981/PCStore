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
        private Dictionary<Guid, int> _quantityInStock;

        public Dictionary<Guid, int> GetQuantityInStock { get { return _quantityInStock; } set { _quantityInStock = value; } }

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository,
                                   IRepository<Product> productRepository,
                                   IDiscountCardService discountCardService,
        ILogger<ShoppingCartService> logger)
        {
            _productRepository = productRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _discountCardService = discountCardService;
            _logger = logger;
            _quantityInStock = new();
        }

        public void AddProduct(Buyer buyer, int productId)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id);
            var selectedProduct = _productRepository.GetAll().FirstOrDefault(p => p.IntId ==productId);

            if (selectedProduct == null)
            {
                _logger.LogWarning($"Product with ID:{productId} not found");
                return;
            }

            if (!_quantityInStock.Any(id => id.Key == selectedProduct.Id))
            {
                _quantityInStock.Add(selectedProduct.Id, selectedProduct.Quantity);
            }

            if (selectedProduct.Quantity > 0)
            {
                var products = shoppingCart.Products;
                if (products.Any(p => p.Id == selectedProduct.Id))
                {
                    products.FirstOrDefault(p => p.Id == selectedProduct.Id)!.Quantity++;
                }
                else
                {
                    products.Add(new Product(selectedProduct.Id, selectedProduct.IntId, selectedProduct.Name,
                                                    selectedProduct.Description, selectedProduct.Price, 1));
                }
                selectedProduct.Quantity--;
            }
            _logger.LogInformation($"Product with ID:{selectedProduct.Id} has been added to shopping cart");
        }

        public void UpdateQuantityProduct(Buyer buyer, int productId, int quantity)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id);
            var products = shoppingCart.Products;
            if (products == null)
            {
                _logger.LogWarning($"There are no items in the cart of the user with ID:{buyer.Id}");
                return;
            }
            if (!products.Any(p => p.IntId == productId))
            {
                throw new Exception($"Product with ID:{productId} not found");
            }

            var product = _productRepository.GetAll().FirstOrDefault(p => p.IntId == productId);
            if (product == null)
            {
                return;
            }
            var quantityProduct = _quantityInStock.FirstOrDefault(x => x.Key == product.Id).Value;
            var oldQuantityValue = products.FirstOrDefault(p => p.Id == product.Id)?.Quantity;

            if (quantity > 0 && quantityProduct - quantity >= 0)
            {
                products.FirstOrDefault(p => p.Id == product.Id)!.Quantity = quantity;
                _productRepository.Update(new Product(product.Id, product.IntId, product.Name, product.Description,
                                               product.Price, quantityProduct - quantity));
            }
            _logger.LogInformation($"The quantity of product with ID:{productId} has been updated from {oldQuantityValue} to {quantity} pieces to the shopping cart");
        }

        public void DeleteProduct(Buyer buyer, int productId)
        {
            var shoppingCart = _shoppingCartRepository.GetByUserId(buyer.Id);

            if (!shoppingCart.Products.Any(p => p.IntId == productId))
            {
                throw new Exception($"Product with ID:{productId} not found");
            }

            var product = shoppingCart.Products.FirstOrDefault(p => p.IntId == productId);
            var productFromDB = _productRepository.GetAll().FirstOrDefault(p => p.IntId == productId);

            if (productFromDB != null && product != null)
            {
                productFromDB.Quantity += product.Quantity;
                _productRepository.Update(productFromDB);
                shoppingCart.Products.Remove(product);
            }
            _logger.LogInformation($"Product with ID:{productId} has been removed from the shopping cart");
        }

        public int CalculateTotalAmount(Buyer buyer)
        {
            var discountCard = buyer.DiscountCards?.OrderByDescending(d => d.Discount).FirstOrDefault();
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
                    GetQuantityInStock.Clear();
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
