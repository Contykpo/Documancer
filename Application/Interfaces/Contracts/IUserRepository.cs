using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;

namespace Application.Interfaces.Contracts
{
    public interface IUserRepository
    {
        Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO registerUserDTO);
        Task<LoginResponse> LoginUserAsync(LoginUserDTO loginUserDTO);
        Task<UpdateUserCampaignsResponse> UpdateUserCampaignsAsync(UpdateUserCampaignsDTO updateUserDTO);
    }
}