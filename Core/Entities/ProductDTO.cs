using Core.Data;

namespace Core.Entities
{
    public class ProductDTO
    {
        public ProductDTO(int id, string name, int price, int quantity)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public int Id { get; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }

        public override string? ToString()
        {
            return string.Format("{0,-7} {1,-45} {2,7} {3,6}", Id, Name, Price, Quantity);
        }
    }
}
