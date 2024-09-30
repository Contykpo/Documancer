using System.ComponentModel.DataAnnotations.Schema;

namespace Documancer.Domain.Identity
{
    public class ApplicationUser : IdentityUser
    {
        #region Properties and Fields

        public string? DisplayName { get; set; }
        public string? Provider { get; set; } = "Local";
        public string? TenantId { get; set; }
        public virtual Tenant? Tenant { get; set; }

        [Column(TypeName = "text")]
        public string? ProfilePictureDataUrl { get; set; }

        public bool IsActive { get; set; }
        public bool IsLive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<ApplicationUserClaim> UserClaims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }

        public string? SuperiorId { get; set; } = null;
        public ApplicationUser? Superior { get; set; } = null;

        #endregion

        #region Constructors

        public ApplicationUser()
        {
            UserClaims = new HashSet<ApplicationUserClaim>();
            UserRoles = new HashSet<ApplicationUserRole>();
            Logins = new HashSet<ApplicationUserLogin>();
            Tokens = new HashSet<ApplicationUserToken>();
        }

        #endregion
    }
}