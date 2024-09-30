using Documancer.Domain.Common.Entities;

namespace Documancer.Domain.Entities
{
    public class Contact : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }

    }
}