using Application.Features.CampaignFeatures.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.AuthenticationFeatures.DataTransferObjects
{
    public class UserDataDTO
    {
        [EmailAddress, Required, DataType(DataType.EmailAddress)]
        [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+", ErrorMessage = "Invalid Email. Please provide an email that follows the structure: example@mail.com")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
