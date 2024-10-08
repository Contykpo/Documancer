using Domain.Common;
using Domain.Entities.Authentication;
using Domain.Entities.Files;
using System;

namespace Domain.Entities.Campaigns
{
    public class Campaign : BaseEntity
    {
        #region Properties

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Campaign banner to be shown on Card.
        /// </summary>
        public virtual Image? BannerImage { get; set; } 

        /// <summary>
        /// Owner of this Campaign.
        /// </summary>
        public virtual ApplicationUser OwnerUser { get; set; }

        #endregion

        #region Constructors

        // Parameterless constructor for EF Core.
        private Campaign() { }

        public Campaign(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        #endregion
    }
}
