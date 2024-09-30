namespace Documancer.Domain.Events
{
    public class ContactUpdatedEvent : DomainEvent
    {
        public ContactUpdatedEvent(Contact item)
        {
            Item = item;
        }

        public Contact Item { get; }
    }
}