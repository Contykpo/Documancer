namespace Application.Features.NarratorFeatures.DataTransferObjects
{
    public class NarratorMessageDTO
    {
        public Guid Id { get; set; }
        public Guid OwnerNarratorId { get; set; }
        public Guid OwnerCampaignId { get; set; }

        public string ConversationId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string AssociatedPrompt { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public DateTimeOffset Timestamp { get; set; }
    }
}
