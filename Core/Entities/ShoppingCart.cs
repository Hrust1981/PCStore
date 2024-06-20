namespace Core.Entities
{
    public class ShoppingCart
    {
        private readonly List<ProductDTO> _products;

        public ShoppingCart(List<ProductDTO> products, Guid userId)
        {
            Id = Guid.NewGuid();
            _products = products;
            UserId = userId;
        }

        public Guid Id { get; }
        public Guid UserId { get; set; }
        public List<ProductDTO> Products { get; set; }
    }
}
