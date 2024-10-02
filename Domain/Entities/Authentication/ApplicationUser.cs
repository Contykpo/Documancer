using Domain.Common;

namespace Domain.Entities.Authentication
{
    public class ApplicationUser : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
