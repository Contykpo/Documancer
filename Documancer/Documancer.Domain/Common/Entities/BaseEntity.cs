﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Documancer.Domain.Common.Entities
{
    public abstract class BaseEntity : IEntity<int>
    {
        #region Properties and Fields

        private readonly List<DomainEvent> _domainEvents = new();

        [NotMapped]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public virtual int Id { get; set; }

        #endregion

        #region Methods

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        #endregion
    }
}