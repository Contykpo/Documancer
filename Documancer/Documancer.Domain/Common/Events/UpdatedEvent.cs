using Documancer.Domain.Common.Entities;

namespace Documancer.Domain.Common.Events
{
    public class UpdatedEvent<T> : DomainEvent where T : IEntity
    {
        public T Entity { get; }

        public UpdatedEvent(T entity)
        {
            Entity = entity;
        }
    }
}