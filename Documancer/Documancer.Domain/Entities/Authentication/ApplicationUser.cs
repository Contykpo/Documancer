using Microsoft.AspNetCore.Identity;

namespace Documancer.Domain.Entities.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
