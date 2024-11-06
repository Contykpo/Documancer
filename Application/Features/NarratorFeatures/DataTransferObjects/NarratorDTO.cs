using Domain.Entities.Campaigns;

namespace Application.Features.NarratorFeatures.DataTransferObjects
{
    public class NarratorDTO
    {
        public Guid Id { get; set; }
        public Guid OwnerCampaignId { get; set; }

        public string GPTConversationId { get; set; } = string.Empty;
        
        public DateTimeOffset CreatedDate { get; set; }

        public List<NarratorMessageDTO> Messages { get; set; } = new List<NarratorMessageDTO>();
    }
}
