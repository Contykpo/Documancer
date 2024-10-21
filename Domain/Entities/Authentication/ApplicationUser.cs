using Domain.Common;
using Domain.Entities.Campaigns;
using System.Collections.Generic;

namespace Domain.Entities.Authentication
{
    public class ApplicationUser : BaseEntity
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        /// <summary>
        /// Active Campaigns managed by the User.
        /// </summary>
        public virtual List<Campaign> Campaigns { get; set; } = new List<Campaign>();
    }
}
