﻿using Documancer.Application.Features.Documents.Caching;

namespace Documancer.Application.Features.Documents.Commands.Upload
{
    public class UploadDocumentCommand : ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        public List<UploadRequest> UploadRequests { get; set; }

        public CancellationTokenSource? SharedExpiryTokenSource => DocumentCacheKey.GetOrCreateTokenSource();

        #endregion

        #region Constructors

        public UploadDocumentCommand(List<UploadRequest> uploadRequests)
        {
            UploadRequests = uploadRequests;
        }

        #endregion
    }

    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        #endregion

        #region Constructors

        public UploadDocumentCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IUploadService uploadService
        )
        {
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {
            var list = new List<Document>();

            foreach (var uploadRequest in request.UploadRequests)
            {
                var fileName = uploadRequest.FileName;
                var url = await _uploadService.UploadAsync(uploadRequest);
                var document = new Document
                {
                    Title = fileName,
                    URL = url,
                    Status = JobStatus.Queueing,
                    IsPublic = true,
                    DocumentType = DocumentType.Image
                };

                document.AddDomainEvent(new CreatedEvent<Document>(document));

                list.Add(document);
            }

            if (!list.Any())
            {
                return await Result<int>.SuccessAsync(0);
            }

            await _context.Documents.AddRangeAsync(list, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(result);
        }

        #endregion
    }
}