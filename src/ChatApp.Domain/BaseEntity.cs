namespace ChatApp.Domain
{
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
        }

        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
