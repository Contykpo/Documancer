using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FilesFeatures.DataTransferObjects
{
    public record ImageDTO(Guid Id, string FileName, string ContentType, byte[] Data);
}
