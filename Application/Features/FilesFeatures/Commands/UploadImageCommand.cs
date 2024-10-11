using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FilesFeatures.Commands
{
    public record UploadImageCommand(string FileName, string ContentType, byte[] Data) : IRequest<Guid>;
}
