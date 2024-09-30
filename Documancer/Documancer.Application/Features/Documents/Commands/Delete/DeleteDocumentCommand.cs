using Documancer.Application.Features.Documents.Caching;

namespace Documancer.Application.Features.Documents.Commands.Delete
{
    public class DeleteDocumentCommand : ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        public int[] Id { get; set; }
        public CancellationTokenSource? SharedExpiryTokenSource => DocumentCacheKey.GetOrCreateTokenSource();

        #endregion

        #region Constructors

        public DeleteDocumentCommand(int[] id)
        {
            Id = id;
        }

        #endregion

    }

    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;

        #endregion

        #region Constructors

        public DeleteDocumentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Documents.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                item.AddDomainEvent(new DeletedEvent<Document>(item));
                _context.Documents.Remove(item);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(result);
        }

        #endregion
    }
}