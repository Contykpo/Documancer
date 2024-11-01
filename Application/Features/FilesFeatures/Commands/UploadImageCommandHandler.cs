﻿using Application.Interfaces;
using Domain.Entities.Files;
using MediatR;

namespace Application.Features.FilesFeatures.Commands
{
    public class UploadImageCommandHandler(IApplicationDbContext context) : IRequestHandler<UploadImageCommand, Guid>
    {
        // DEPRECATED: Might want to update in the future with a new Image entity for sessions.

        //public async Task<Guid> Handle(UploadImageCommand command, CancellationToken cancellationToken)
        //{
        //    var image = new Image
        //    {
        //        FileName = command.FileName,
        //        ContentType = command.ContentType,
        //        Data = command.Data,

        //    };
        //    await context.Images.AddAsync(image);
        //    await context.SaveChangesAsync();

        //    return image.Id;
        //}
        public Task<Guid> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
