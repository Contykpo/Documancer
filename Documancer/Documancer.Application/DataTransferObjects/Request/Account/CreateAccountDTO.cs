using System.ComponentModel.DataAnnotations;

namespace Documancer.Application.DataTransferObjects.Request.Account
{
    public class CreateAccountDTO : LoginDTO
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; } = string.Empty;
        [Required, Compare(nameof(Password))]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Role")]
        public string Role = string.Empty;
    }
}
