namespace Core.Entities
{
    public class ShoppingCart : Entity
    {
        public ShoppingCart(List<ProductDTO> products, Guid userId)
        {
            Id = Guid.NewGuid();
            Products = products;
            UserId = userId;
        }

        public Guid UserId { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
