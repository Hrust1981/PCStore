using Core.Entities;
using Core.Repositories;
using Core.Services;
using Moq;

namespace PCStore.UnitTests
{
    public class ShoppingCartServiceTests
    {
        private Buyer user = new Buyer(
            Guid.Parse("508d5b6d-e0b5-440b-b91d-dfd7640f4fb0"),
            "TestUser",
            "TestUser",
            "TestPassword",
            "test@test.ru",
            Core.Enumerations.Role.Buyer);
        private static List<Product> products = new List<Product>()
        {
            new Product("Product1", "Product1", 1, 1),
            new Product("Product2", "Product2", 1, 1),
            new Product("Product3", "Product3", 1, 1)
        };
        private Product product = new Product("Product", "Product", 10000, 1);
        private ShoppingCart cart = new ShoppingCart(
            Guid.Parse("06e2d464-cdf5-4143-9676-8783b40bdb90"),
            products,
            Guid.Parse("508d5b6d-e0b5-440b-b91d-dfd7640f4fb0"));

        [Fact]
        public void AddProduct()
        {
            var mock = new Mock<IShoppingCartService>();
            mock.Setup(service => service.AddProduct(user, product));
        }

    }
}
