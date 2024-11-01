﻿using Domain.Common;
using Domain.Entities.Campaigns;
using System;
using System.Xml.Linq;

namespace Domain.Entities.Files
{
    public class Image : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[]? Data { get; set; }

        /// <summary>
        /// Foreign key for owner Campaign.
        /// </summary>
        public Guid? OwnerCampaignId { get; set; }

        /// <summary>
        /// Owner Campaign of this Image.
        /// </summary>
        public Campaign? OwnerCampaign { get; set; } = null!;


        #region Constructors

        // Parameterless constructor for EF Core.
        private Image() { }

        public Image(string fileName, string contentType, byte[]? data, Campaign? owner)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            ContentType = contentType;
            Data = data;

            if (owner != null)
            {
                owner.BannerImage = this;
            
                OwnerCampaignId = owner.Id;
                OwnerCampaign = owner;
            }
        }

        #endregion
    }
}
