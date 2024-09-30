using Documancer.Application.Common.Interfaces.MultiTenant;
using Documancer.Application.Features.Tenants.Caching;

namespace Documancer.Application.Features.Tenants.Commands.Delete
{
    public class DeleteTenantCommand : ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        public string[] Id { get; }
        public string CacheKey => TenantCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => TenantCacheKey.GetOrCreateTokenSource();

        #endregion

        #region Constructors

        public DeleteTenantCommand(string[] id)
        {
            Id = id;
        }

        #endregion
    }

    public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<DeleteTenantCommandHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantsService;

        #endregion

        #region Constructors

        public DeleteTenantCommandHandler(
            ITenantService tenantsService,
            IApplicationDbContext context,
            IStringLocalizer<DeleteTenantCommandHandler> localizer,
            IMapper mapper
        )
        {
            _tenantsService = tenantsService;
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            var items = await _context.Tenants.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                _context.Tenants.Remove(item);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            _tenantsService.Refresh();

            return await Result<int>.SuccessAsync(result);
        }

        #endregion
    }
}