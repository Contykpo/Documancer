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
        /// Owner of this Campaign.
        /// </summary>
        public virtual ApplicationUser OwnerUser { get; set; }

        /// <summary>
        /// Campaign banner to be shown on Card.
        /// </summary>
        public virtual Image BannerImage { get; set; }

        #endregion

        #region Constructors

        // Parameterless constructor for EF Core.
        private Campaign() { }

        public Campaign(string name, string description, Image? bannerImage)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            BannerImage = bannerImage!;

            if (bannerImage != null)
            {
                bannerImage.OwnerCampaign = this;
            }
        }

        #endregion
    }
}
