using Domain.Common;
using Domain.Entities.Campaigns;
using System;

namespace Domain.Entities.Files
{
    public class Image : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[]? Data { get; set; }

        /// <summary>
        /// Foreign key for Campaign.
        /// </summary>
        public Guid? OwnerCampaignId { get; set; }

        /// <summary>
        /// Owner of this Campaign.
        /// </summary>
        public virtual Campaign? OwnerCampaign { get; set; }
    }
}
