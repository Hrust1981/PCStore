namespace Core.Entities
{
    public abstract class Entity
    {
        protected Entity(string name)
        {
            Name = name;
        }

        //public abstract int Id { get; }
        public Guid Id { get; private protected set; }
        public string Name { get; set; }
    }
}
