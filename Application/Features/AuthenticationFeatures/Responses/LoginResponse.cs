namespace Application.Features.AuthenticationFeatures.Responses
{
    public record LoginResponse(bool Flag = false, string Message = null!, string Token = null!);
}
