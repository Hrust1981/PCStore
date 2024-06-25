namespace Core.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; private protected set; }
        public string Name { get; set; }
    }
}
