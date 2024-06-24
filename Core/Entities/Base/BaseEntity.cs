using MediatR;

namespace Core.Entities.Base
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
        public List<INotification> DomainEvents { get; } = [];
    }
}
