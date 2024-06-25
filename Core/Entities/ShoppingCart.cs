namespace Core.Entities
{
    public class ShoppingCart : Entity
    {
        public ShoppingCart(List<Product> products, Guid userId)
        {
            Id = Guid.NewGuid();
            Products = products;
            UserId = userId;
        }

        public Guid UserId { get; set; }
        public List<Product> Products { get; set; }
    }
}
