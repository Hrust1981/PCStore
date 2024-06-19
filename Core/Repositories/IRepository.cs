namespace Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(Guid id);
        List<T> GetAll();
        void Add(T t);
        void Update(T t);
        void Delete(Guid id);
    }
}

