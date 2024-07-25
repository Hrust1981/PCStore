using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace PCStore.UnitTests
{
    public class ShoppingCartServiceTests
    {
        [Fact]
        public void AddProduct()
        {
            // Arrange
            Buyer user = new Buyer(
                Guid.Parse("508d5b6d-e0b5-440b-b91d-dfd7640f4fb0"),
                "TestUser",
                "TestUser",
                "TestPassword",
                "test@test.ru",
                Core.Enumerations.Role.Buyer);

            List<Product> products = new List<Product>();

            ShoppingCart cart = new ShoppingCart(
                Guid.Parse("06e2d464-cdf5-4143-9676-8783b40bdb90"),
                products,
                Guid.Parse("508d5b6d-e0b5-440b-b91d-dfd7640f4fb0"));

            var product = new Product(Guid.Parse("92a612f7-cd4d-4183-ba11-d87475a68a00"), "product", "product", 100, 1);
            var shoppingCartRepository = new Mock<IShoppingCartRepository>();
            var dictionary = new Dictionary<Guid, int>
            {
                { Guid.Parse("ce3577b8-247d-4f74-a1dc-a9b48b7bd198"), 1 }
            };
            shoppingCartRepository.Setup(q => q.QuantityInStock).Returns(dictionary);
            shoppingCartRepository.Setup(u => u.GetByUserId(Guid.Parse("508d5b6d-e0b5-440b-b91d-dfd7640f4fb0"))).Returns(cart);

            var productRepository = new Mock<IRepository<Product>>();
            productRepository.Setup(p => p.Get(Guid.Parse("92a612f7-cd4d-4183-ba11-d87475a68a00"))).Returns(product);

            var discountCardService = new Mock<IDiscountCardService>();
            var logger = new Mock<ILogger<ShoppingCartService>>();

            var shoppingCartService = new ShoppingCartService(
                shoppingCartRepository.Object,
                productRepository.Object,
                discountCardService.Object,
                logger.Object);


            // Act
            shoppingCartService.AddProduct(user, product);


            // Assert
            Assert.NotEmpty(products);
            Assert.Equal(product.Id, products.First()?.Id);
        }

    }
}
