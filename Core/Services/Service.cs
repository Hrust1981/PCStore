using AutoMapper;
using Core.Entities;
using Core.Repositories;

namespace Core.Services
{
    public class Service : IService
    {
        private readonly IProductRepository _repository;

        public Service(IProductRepository repository)
        {
            _repository = repository;
        }

        public void Add(ProductDTO product)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, Product>());
            var mapper = config.CreateMapper();
            var productDTO = mapper.Map<Product>(product);
            _repository.Add(productDTO);
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
