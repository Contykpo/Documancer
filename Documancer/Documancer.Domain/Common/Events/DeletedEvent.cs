using Documancer.Domain.Common.Entities;

namespace Documancer.Domain.Common.Events
{
    public class DeletedEvent<T> : DomainEvent where T : IEntity
    {
        public T Entity { get; }

        public DeletedEvent(T entity)
        {
            Entity = entity;
        }
    }
}