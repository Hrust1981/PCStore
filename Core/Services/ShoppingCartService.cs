using Core.Entities;
using Core.Repositories;
using System;

namespace Core.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IProductRepository _repository;

        public ShoppingCartService(IProductRepository repository)
        {
            _repository = repository;
        }

        public void Add(ProductDTO product, Buyer buyer, int valueId)
        {
            if (valueId > 0 && valueId <= _repository.Count)
            {
                var selectedProduct = _repository.Get(valueId);
                if (selectedProduct.Quantity > 0)
                {
                    var shoppingCart = buyer.ShoppingCart;
                    if (shoppingCart.Any(p => p.Id == valueId))
                    {
                        shoppingCart.SingleOrDefault(p => p.Id == valueId).Quantity++;
                    }
                    else
                    {
                        shoppingCart.Add(new ProductDTO(selectedProduct.Name, selectedProduct.Price, 1));
                    }
                    selectedProduct.Quantity--;
                }
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductDTO product)
        {
            throw new NotImplementedException();
        }
    }
}
