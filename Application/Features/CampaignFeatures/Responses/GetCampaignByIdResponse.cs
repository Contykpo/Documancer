using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CampaignFeatures.Responses
{
    public record GetCampaignByIdResponse(Guid Id, bool Flag = false, string Message = null!);
}
