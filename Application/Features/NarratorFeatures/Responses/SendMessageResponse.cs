using Application.Features.NarratorFeatures.DataTransferObjects;

namespace Application.Features.NarratorFeatures.Responses
{
    public record SendMessageResponse(bool Flag = false, string Message = null!, NarratorMessageDTO NarratorMessage = null!);
}
