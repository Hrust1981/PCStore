using Core.Data;

namespace Core.Entities
{
    public class ProductDTO : Entity
    {
        public ProductDTO(Guid id, string name, int price, int quantity) : base(name)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        //public override int Id { get; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public override string? ToString()
        {
            return string.Format("{0,-7} {1,-45} {2,7} {3,6}", Id, CutText(Name, 40), Price, Quantity);
        }

        private string CutText(string text, int length)
        {
            return text.Length > length ? text.Substring(0, length) : text;
        }
    }
}
