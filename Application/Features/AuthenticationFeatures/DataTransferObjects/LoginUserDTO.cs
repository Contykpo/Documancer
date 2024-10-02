using System.ComponentModel.DataAnnotations;

namespace Application.Features.AuthenticationFeatures.DataTransferObjects
{
    public class LoginUserDTO
    {
        [EmailAddress, Required, DataType(DataType.EmailAddress)]
        [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+", ErrorMessage = "Invalid Email. Please provide an email that follows the structure: example@mail.com")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$", ErrorMessage = "Password must have alphanumeric and special characters.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
    }
}
