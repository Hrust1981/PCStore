namespace Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        List<T> GetAll();
        void Add(T t);
        void Update(T t);
        void Delete(int id);
    }
}

