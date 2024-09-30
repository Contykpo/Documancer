namespace Documancer.Domain.Identity
{
    public class ApplicationRole : IdentityRole
    {
        #region Properties and Fields

        public string? TenantId { get; set; }
        public virtual Tenant? Tenant { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        #endregion

        #region Constructors

        public ApplicationRole()
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            UserRoles = new HashSet<ApplicationUserRole>();
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
            RoleClaims = new HashSet<ApplicationRoleClaim>();
            UserRoles = new HashSet<ApplicationUserRole>();
        }

        #endregion
    }
}