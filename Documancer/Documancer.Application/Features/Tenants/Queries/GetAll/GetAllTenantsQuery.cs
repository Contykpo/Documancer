using Documancer.Application.Features.Tenants.Caching;
using Documancer.Application.Features.Tenants.DTOs;

namespace Documancer.Application.Features.Tenants.Queries.GetAll
{
    public class GetAllTenantsQuery : ICacheableRequest<IEnumerable<TenantDto>>
    {
        public string CacheKey => TenantCacheKey.GetAllCacheKey;
        public MemoryCacheEntryOptions? Options => TenantCacheKey.MemoryCacheEntryOptions;
    }

    public class GetAllTenantsQueryHandler :
        IRequestHandler<GetAllTenantsQuery, IEnumerable<TenantDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<GetAllTenantsQueryHandler> _localizer;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public GetAllTenantsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllTenantsQueryHandler> localizer
        )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<TenantDto>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Tenants.OrderBy(x => x.Name).ProjectTo<TenantDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return data;
        }

        #endregion
    }
}