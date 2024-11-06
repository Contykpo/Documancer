using Domain.Common;
using System;

namespace Domain.Entities.Campaigns
{
    public class NarratorMessage : BaseEntity
    {
        #region Properties

        public string ConversationId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public DateTimeOffset Timestamp { get; set; }

        #endregion

        #region Owner Campaign Narration

        public Guid OwnerNarratorId { get; set; }

        public Narrator OwnerNarrator { get; set; } = null!;

        #endregion


        #region Constructors

        // Parameterless constructor for EF Core.
        private NarratorMessage() { }

        public NarratorMessage(Guid ownerNarratorId, string conversationId, string role, string content, DateTimeOffset timeStamp)
        {
            Id = Guid.NewGuid();
            OwnerNarratorId = ownerNarratorId;

            ConversationId = conversationId;
            Content = content;
            Role = role;

            Timestamp = timeStamp;
        }

        #endregion
    }
}
