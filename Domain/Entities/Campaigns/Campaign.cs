using Domain.Common;
using Domain.Entities.Authentication;
using Domain.Entities.Files;
using System;

namespace Domain.Entities.Campaigns
{
    public class Campaign : BaseEntity
    {
        #region Campaign Properties

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Optional Campaign banner to be shown on Card.
        /// </summary>
        public Image? BannerImage { get; set; } = null!;

        #endregion

        #region Owner User Properties

        /// <summary>
        /// Foreign key for owner User of this Campaign.
        /// </summary>
        public Guid? OwnerUserId { get; set; }

        /// <summary>
        /// Owner of this Campaign.
        /// </summary>
        public ApplicationUser OwnerUser { get; set; } = null!;

        #endregion

        #region Constructors

        // Parameterless constructor for EF Core.
        private Campaign() { }

        public Campaign(string name, string description, ApplicationUser ownerUser, Image? bannerImage)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            
            OwnerUserId = ownerUser.Id;
            OwnerUser = ownerUser;
            
            BannerImage = bannerImage!;

            if (bannerImage != null)
            {
                bannerImage.OwnerCampaign = this;
            }
        }

        #endregion
    }
}
