using Core.Data;

namespace Core.Entities
{
    public class ProductDTO
    {
        public ProductDTO(string name, int price, int quantity)
        {
            Id = DB.CounterProductDTOId;
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
            return Id + "    " + Name + "|    " + Price + "|    " + Quantity;
        }
    }
}
