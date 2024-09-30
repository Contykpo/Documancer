using Documancer.Application.Features.Contacts.Caching;

namespace Documancer.Application.Features.Contacts.Commands.Delete
{
    public class DeleteContactCommand:  ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        public int[] Id {  get; }
        public string CacheKey => ContactCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => ContactCacheKey.GetOrCreateTokenSource();

        #endregion

        #region Constructors

        public DeleteContactCommand(int[] id)
        {
            Id = id;
        }

        #endregion
    }

    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<DeleteContactCommandHandler> _localizer;

        #endregion

        #region Constructors

        public DeleteContactCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<DeleteContactCommandHandler> localizer,
            IMapper mapper
        )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Contacts.Where(x=>request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
            foreach (var item in items)
            {
			    // Raise a delete domain event:
				item.AddDomainEvent(new ContactDeletedEvent(item));
                
                _context.Contacts.Remove(item);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
 
            return await Result<int>.SuccessAsync(result);
        }

        #endregion
    }
}