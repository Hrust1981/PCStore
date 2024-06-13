﻿using Core.Data;

namespace Core.Entities
{
    public class Product : Entity
    {
        public Product(string name, string description, int price, int quantity)
        {
            Id = DB.CounterProductId;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }
        public Product(int id, string name, string description, int price, int quantity)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public override int Id { get; }
        public override string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Quantity { get; set; }

        public override string? ToString()
        {
            return string.Format("{0,-4} {1,-35} {2,-60} {3,7} {4,6}", Id,
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
