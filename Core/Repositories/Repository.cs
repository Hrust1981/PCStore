using AutoMapper;
using Core.Entities;

namespace Core.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly List<T> _entities;

        public Repository(List<T> entities)
        {
            _entities = entities;
        }

        public void Add(T entity)
        {
            if (_entities.Any(s => s.Id == entity.Id))
            {
                throw new Exception($"Entity with ID:{entity.Id} already exists");
            }
            _entities.Add(entity);
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception($"Entity with ID:{id} not found");
            }
            _entities.Remove(entity);
        }

        public T Get(Guid id)
        {
            var entity = _entities.FirstOrDefault(s => s.Id == id);
            if (entity == null)
            {
                throw new Exception($"Entity with ID:{id} not found");
            }
            return entity;
        }

        public List<T> GetAll()
        {
            return _entities;
        }

        public void Update(T entity)
        {
            var updateEntity = Get(entity.Id);
            if (updateEntity == null)
            {
                throw new Exception($"Entity with ID:{entity.Id} not found");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, T>());
            var mapper = config.CreateMapper();
            mapper.Map<T, T>(entity, updateEntity);
        }
    }
}
