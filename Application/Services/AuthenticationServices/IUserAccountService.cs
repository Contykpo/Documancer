using Application.Features.AuthenticationFeatures.DataTransferObjects;
using Application.Features.AuthenticationFeatures.Responses;

namespace Application.Services.AuthenticationServices
{
    public interface IUserAccountService
    {
        Task<RegistrationResponse> RegisterUserAccountAsync(RegisterUserDTO registerUserDTO);
        Task<LoginResponse> LogInUserAccountAsync(LoginUserDTO loginUserDTO);
        Task<UpdateUserCampaignsResponse> UpdateUserAccountAsync(UpdateUserCampaignsDTO updateUserDTO);
    }
}
