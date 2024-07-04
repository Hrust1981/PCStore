using Core.Repositories;
using Core.Entities;

namespace Tests
{
    public class ProductRepositoryTests
    {
        private List<Product> prvGetListProduct()
        {
            return new List<Product>()
            {
                new Product(Guid.Parse("1e720551-e86e-47f7-bf4e-dcbf8773759f"), "PC DEXP Aquilon O296", "Intel Core i3-10100, 4 x 3.6 ГГц, 8 ГБ DDR4, SSD 256 ГБ", 22199, 3),
                new Product(Guid.Parse("f34b41a0-c7d8-4f18-86de-8dba869d9d0a"), "Notebook Lenovo ThinkPad L390", "Intel Core i5-8265U, ядра: 4 х 1.6 ГГц, RAM 8 ГБ", 82499, 4),
                new Product(Guid.Parse("4f4d49dd-1a0f-40c6-ae53-7d643dd314b9"), "Monitor Xiaomi A22i", "1920x1080 (FullHD)@75 Гц, VA, LED", 8999, 5),
                new Product(Guid.Parse("40e7c7b4-92b1-4bf5-a224-ae7d54f1828d"), "Printer Brother HL-1210W", "A4, 2400x600 dpi, USB, Wi-Fi", 9099, 2)
            };
        }

        private IRepository<Product> prvGetProductRepository()
        {
            var listProduct = prvGetListProduct();
            return new ProductRepository(listProduct);
        }

        [Fact]
        public void GetAll_Nothing_ListOfProducts()
        {
            prvGetProductRepository().GetAll();
        }

        [Fact]
        public void Get_Guid_Product()
        {
            prvGetProductRepository().Get(Guid.Parse("1e720551-e86e-47f7-bf4e-dcbf8773759f"));
        }

        [Fact]
        public void Add_Product()
        {
            prvGetProductRepository().Add(new Product("product", "product", 1, 1));
        }

        [Fact]
        public void Update_Product_UpdatableProduct()
        {
            prvGetProductRepository().Update(new Product(Guid.Parse("40e7c7b4-92b1-4bf5-a224-ae7d54f1828d"), "Printer Brother HL-1210W", "A4, 2400x600 dpi, USB, Wi-Fi", 9199, 12));
        }

        [Fact]
        public void Delete_Guid()
        {
            prvGetProductRepository().Delete(Guid.Parse("f34b41a0-c7d8-4f18-86de-8dba869d9d0a"));
        }
    }
}
