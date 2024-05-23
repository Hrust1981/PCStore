namespace Core.Entities
{
    public class Product
    {
        private readonly Guid _id;

        public Product(string name, string description, int price, int quantity)
        {
            _id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}
