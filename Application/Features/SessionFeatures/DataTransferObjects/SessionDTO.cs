using System.ComponentModel.DataAnnotations;

namespace Application.Features.SessionFeatures.DataTransferObjects
{
    public class SessionDTO
    {
        public Guid Id { get; set; }

        [Required]
        public Guid OwnerCampaignId { get; set; }

        [Required]
        public DateTimeOffset CreationDate { get; set; }

        public List<string> Notes { get; set; } = new List<string>();
    }
}
