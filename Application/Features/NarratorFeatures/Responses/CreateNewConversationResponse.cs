namespace Application.Features.NarratorFeatures.Responses
{
    public record CreateNewConversationResponse(bool Flag = false, string Message = null!, string NarratorId = null!);
}
