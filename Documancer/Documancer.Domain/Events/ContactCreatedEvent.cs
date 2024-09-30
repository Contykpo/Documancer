namespace Documancer.Domain.Events
{
    public class ContactCreatedEvent : DomainEvent
    {
        public ContactCreatedEvent(Contact item)
        {
            Item = item;
        }

        public Contact Item { get; }
    }
}