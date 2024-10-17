﻿using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Domain.Entities.Files;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CampaignFeatures.DataTransferObjects
{
    // public record CampaignDTO(Guid Id, string Name, string Description, string? FileName = null, string? ContentType = null, byte[]? Data = null);

    public class CampaignDTO
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Campaign Name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        #region Owner User

        [EmailAddress, Required, DataType(DataType.EmailAddress)]
        [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+", ErrorMessage = "Invalid Email. Please provide an email that follows the structure: example@mail.com")]
        [Display(Name = "Email Address")]
        public string OwnerEmailAddress { get; set; } = string.Empty;

        #endregion

        #region Banner Image Properties

        public string? FileName { get; set; } = null;
        public string? ContentType { get; set; } = null;
        
        public byte[]? Data { get; set; } = null;
        
        #endregion
    }
}
