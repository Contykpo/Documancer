namespace Documancer.Domain.Events
{
    public class ContactDeletedEvent : DomainEvent
    {
        public ContactDeletedEvent(Contact item)
        {
            Item = item;
        }

        public Contact Item { get; }
    }
}