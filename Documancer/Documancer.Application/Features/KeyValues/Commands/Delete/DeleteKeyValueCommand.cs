using Documancer.Application.Features.KeyValues.Caching;

namespace Documancer.Application.Features.KeyValues.Commands.Delete
{
    public class DeleteKeyValueCommand : ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        public int[] Id { get; }
        public string CacheKey => KeyValueCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => KeyValueCacheKey.GetOrCreateTokenSource();

        #endregion

        #region Constructors

        public DeleteKeyValueCommand(int[] id)
        {
            Id = id;
        }

        #endregion
    }

    public class DeleteKeyValueCommandHandler : IRequestHandler<DeleteKeyValueCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;

        #endregion

        #region Constructors

        public DeleteKeyValueCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(DeleteKeyValueCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.KeyValues.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                var changeEvent = new UpdatedEvent<KeyValue>(item);

                item.AddDomainEvent(changeEvent);

                _context.KeyValues.Remove(item);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(result);
        }

        #endregion
    }
}