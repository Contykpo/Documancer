using Documancer.Application.Features.Products.Caching;

namespace Documancer.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommand : ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        public int[] Id { get; }
        public string CacheKey => ProductCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => ProductCacheKey.GetOrCreateTokenSource();

        #endregion

        #region Constructors

        public DeleteProductCommand(int[] id)
        {
            Id = id;
        }

        #endregion
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<DeleteProductCommandHandler> _localizer;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public DeleteProductCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteProductCommandHandler> localizer,
            IMapper mapper
        )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Products.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                item.AddDomainEvent(new DeletedEvent<Product>(item));
                _context.Products.Remove(item);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(result);
        }

        #endregion
    }
}