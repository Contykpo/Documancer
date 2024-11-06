using Domain.Common;
using System.Collections.Generic;
using System;

namespace Domain.Entities.Campaigns
{
    public class Narrator : BaseEntity
    {
        #region Properties

        public string GPTConversationId { get; set; } = string.Empty;
        public DateTimeOffset CreationDate { get; set; }

        public IList<NarratorMessage> Messages { get; set; } = new List<NarratorMessage>();

        #endregion

        #region Owner Campaign Properties

        /// <summary>
        /// Foreign key for owner Campaign of this Session.
        /// </summary>
        public Guid OwnerCampaignId { get; set; }

        /// <summary>
        /// Owner Campaign of this Session.
        /// </summary>
        public Campaign OwnerCampaign { get; set; } = null!;

        #endregion


        #region Constructor

        // Parameterless constructor for EF Core.
        private Narrator() { }

        public Narrator(string gptConversationID, DateTimeOffset creationDate, Guid ownerCampaignId)
        {
            Id = Guid.NewGuid();
            OwnerCampaignId = ownerCampaignId;

            GPTConversationId = gptConversationID;

            CreationDate = creationDate;
        }

        #endregion
    }
}
