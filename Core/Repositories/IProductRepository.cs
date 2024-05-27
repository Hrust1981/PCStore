﻿using Core.Entities;

namespace Core.Repositories
{
    public interface IProductRepository
    {
        int Count { get; }
        void Add(Product product);
        List<Product> GetAll();
        Product Get(int id);
        void Update(Product product);
        void Delete(int id);
    }
}
