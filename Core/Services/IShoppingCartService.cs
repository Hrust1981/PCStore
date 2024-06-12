﻿using Core.Entities;

namespace Core.Services
{
    public interface IShoppingCartService
    {
        public Dictionary<int, int> GetQuantityInStock { get;  set; }
        void AddProduct(Buyer buyer, int productId);
        void UpdateQuantityProduct(Buyer buyer, int productId, int quantity);
        void DeleteProduct(Buyer buyer, int productId);
        int CalculateTotalAmount(Buyer buyer, out int totalAmountWithDiscount);
    }
}
