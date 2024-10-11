using System.ComponentModel.DataAnnotations;

namespace Documancer.API.Features.Campaigns.Requests
{
    public class CreateCampaignRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public IFormFile? BannerImageFile { get; set; }
    }
}
