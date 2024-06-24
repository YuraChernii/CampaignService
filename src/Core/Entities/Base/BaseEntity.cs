using MediatR;

namespace Core.Entities.Base
{
    public abstract class BaseEntity<T>(T id = default)
    {
        public T Id { get; set; } = id;
        public List<INotification> DomainEvents { get; } = [];
    }
}
