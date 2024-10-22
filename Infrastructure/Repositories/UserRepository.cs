using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;
using Application.Features.CampaignFeatures.DataTransferObjects;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Domain.Entities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository(IApplicationDbContext context, IConfiguration configuration) : IUserRepository
    {
        // Interface implementation methods:

        public async Task<LoginResponse> LoginUserAsync(LoginUserDTO loginUserDTO)
        {
            try
            {
                var user = await FindUserByEmailAsync(loginUserDTO.EmailAddress);

                if (user is null) { return new LoginResponse(false, "User not found."); }

                bool checkPassword = BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, user.Password);

                if (checkPassword)
                {
                    return new LoginResponse(true, "Login successfully.", GenerateJWTToken(user));
                }
                else
                {
                    return new LoginResponse(false, "Invalid credentials. Failed to login.");
                }
            }
            catch (Exception exception)
            {
                return new LoginResponse(false, exception.Message);
            }
        }

        public async Task<GetUserDataResponse> GetUserCampaignsAsync(UserDataDTO userDataDTO)
        {
            try
            {
                var user = await FindUserByEmailAsync(userDataDTO.EmailAddress);

                if (user is null) { return new GetUserDataResponse(false, "User not found."); }

                var userCampaigns = (from c in context.Campaigns where c.OwnerUserId.Equals(user.Id) select c).ToList();

                if (userCampaigns.IsNullOrEmpty()) { return new GetUserDataResponse(false, "Campaigns could not be found."); }

                var userCampaignDTOs = new List<CampaignDTO>();

                foreach (var campaign in userCampaigns)
                {
                    var campaignBanner = await context.Images.FirstOrDefaultAsync(i => i.OwnerCampaignId == campaign.Id);

                    userCampaignDTOs.Add(new CampaignDTO
                    {
                        Id = campaign.Id,
                        Name = campaign.Name,
                        Description = campaign.Description,

                        OwnerEmailAddress = userDataDTO.EmailAddress,

                        FileName = campaignBanner != null ? campaignBanner.FileName : string.Empty,
                        ContentType = campaignBanner != null ? campaignBanner.ContentType : string.Empty,
                        Data = campaignBanner != null ? campaignBanner.Data! : []
                    });
                }

                return new GetUserDataResponse(true, $"Successfully retrieved user data. Here is the db count: {userCampaigns.Count}.", userCampaignDTOs);
            }
            catch (Exception exception)
            {
                return new GetUserDataResponse(false, exception.Message);
            }
        }

        public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            try
            {
                if (await FindUserByEmailAsync(registerUserDTO.EmailAddress) != null)
                {
                    return new RegistrationResponse(false, "User already exists.");
                }

                context.Users.Add(new ApplicationUser()
                {
                    Name = registerUserDTO.Name,
                    Email = registerUserDTO.EmailAddress,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerUserDTO.Password)
                });

                await context.SaveChangesAsync();

                return new RegistrationResponse(true, "Registration completed.");
            }
            catch (Exception exception)
            {
                return new RegistrationResponse(false, exception.Message);
            }
        }

        public async Task<UpdateUserCampaignsResponse> UpdateUserCampaignsAsync(UpdateUserCampaignsDTO updateUserDTO)
        {
            try
            {
                var user = await FindUserByEmailAsync(updateUserDTO.EmailAddress);

                if (user == null) return new UpdateUserCampaignsResponse(false, "User does not exist.");
                
                user.Campaigns.Add(updateUserDTO.Campaign!);

                await context.SaveChangesAsync();

                return new UpdateUserCampaignsResponse(true, "New campaign added to user.");
            }
            catch (Exception exception)
            {
                return new UpdateUserCampaignsResponse(false, exception.Message);
            }
        }


        // Class-specific methods:

        private async Task<ApplicationUser> FindUserByEmailAsync(string email) => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

        private string GenerateJWTToken(ApplicationUser user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var userClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name!),
                    new Claim(ClaimTypes.Email, user.Email!)
                };

                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null!;
            }
        }
    }
}
