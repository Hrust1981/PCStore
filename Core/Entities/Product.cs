﻿using Core.Data;

namespace Core.Entities
{
    public class Product : Entity
    {
        public Product(string name, string description, int price, int quantity)/* : base(name)*/
        {
            Id = Guid.NewGuid();
            IntId = DB.CounterProductId;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
        public Product(Guid id, string name, string description, int price, int quantity) /*: base(name)*/
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        //public override int Id { get; }
        public int IntId { get; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public override string? ToString()
        {
            return string.Format("{0,-4} {1,-35} {2,-60} {3,7} {4,6}", IntId,
                                                                       CutText(Name, 30),
                                                                       CutText(Description, 55),
                                                                       Price,
                                                                       Quantity);
        }

        private string CutText(string text, int length)
        {
            return text.Length > length ? text.Substring(0, length) : text;
        }
    }
}
