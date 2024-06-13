namespace Core.Entities
{
    public abstract class Entity
    {
        public abstract int Id { get; }
        public abstract string Name { get; set; }
    }
}
