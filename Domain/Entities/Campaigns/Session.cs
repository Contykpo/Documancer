using Domain.Common;
using System;
using System.Collections.Generic;

namespace Domain.Entities.Campaigns
{
    public class Session : BaseEntity
    {
        #region Session Properties

        /// <summary>
        /// The date the session started.
        /// We take the current day date whenever a new <see cref="Session"/> gets created.
        /// </summary>
        public DateTimeOffset CreationDate { get; set; }

        /// <summary>
        /// Notes for every event that took place during the session.
        /// TODO: Might want to evolve this property in the future to a new entity class, such as <see cref="StoryNote"/>.
        /// </summary>
        public ICollection<string> Notes { get; set; } = new List<string>();

        #endregion

        #region Owner Campaign Properties

        /// <summary>
        /// Foreign key for owner User of this Campaign.
        /// </summary>
        public Guid? OwnerCampaignId { get; set; }

        /// <summary>
        /// Owner of this Campaign.
        /// </summary>
        public Campaign OwnerCampaign { get; set; } = null!;

        #endregion


        #region Constructors

        // Parameterless constructor for EF Core.
        private Session() { }

        public Session(DateTimeOffset creationDate, Guid ownerCampaignId)
        {
            Id = Guid.NewGuid();
            OwnerCampaignId = ownerCampaignId;

            CreationDate = creationDate;
        }

        #endregion
    }
}
