using Core.Data;

namespace Core.Entities
{
    public class Product
    {
        private int _quantity;
        public Product(string name, string description, int price, int quantity)
        {
            Id = DB.CounterProductId;
            Name = name;
            Description = description;
            Price = price;
            _quantity = quantity;
        }
        public Product(int id, string name, string description, int price, int quantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            _quantity = quantity;
        }

        public int Id { get; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get => _quantity; set { _quantity = value;  } }

        public override string? ToString()
        {
            return Id + "    " + Name + "|    " + Description + "|    " + Price + "|    " + Quantity;
        }
    }
}
