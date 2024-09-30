using Documancer.Domain.Common.Entities;

namespace Documancer.Domain.Common.Events
{
    public class CreatedEvent<T> : DomainEvent where T : IEntity
    {
        public T Entity { get; }

        public CreatedEvent(T entity)
        {
            Entity = entity;
        }
    }
}