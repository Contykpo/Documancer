using MediatR;

namespace Documancer.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        #region Properties and Fields

        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccurred { get; protected set; }

        #endregion

        #region Constructors

        protected DomainEvent()
        {
            DateOccurred = DateTimeOffset.UtcNow;
        }

        #endregion
    }
}