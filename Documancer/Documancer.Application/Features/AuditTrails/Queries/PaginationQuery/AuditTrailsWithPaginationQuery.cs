using Documancer.Application.Features.AuditTrails.Caching;
using Documancer.Application.Features.AuditTrails.DTOs;
using Documancer.Application.Features.AuditTrails.Specifications;

namespace Documancer.Application.Features.AuditTrails.Queries.PaginationQuery
{
    public class AuditTrailsWithPaginationQuery : AuditTrailAdvancedFilter, ICacheableRequest<PaginatedData<AuditTrailDto>>
    {
        #region Properties and Fields

        public int LocalTimezoneOffset { get; set; }
        public AuditTrailAdvancedSpecification Specification => new(this);

        public string CacheKey => AuditTrailsCacheKey.GetPaginationCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => AuditTrailsCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Listview:{ListView}-{LocalTimezoneOffset},AuditType:{AuditType},Search:{Keyword},Sort:{SortDirection},OrderBy:{OrderBy},{PageNumber},{PageSize}";
        }

        #endregion
    }

    public class AuditTrailsQueryHandler : IRequestHandler<AuditTrailsWithPaginationQuery, PaginatedData<AuditTrailDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public AuditTrailsQueryHandler(
            ICurrentUserService currentUserService,
            IApplicationDbContext context,
            IMapper mapper
        )
        {
            _currentUserService = currentUserService;
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<AuditTrailDto>> Handle(AuditTrailsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.AuditTrails.OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectToPaginatedDataAsync<AuditTrail, AuditTrailDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

            return data;
        }

        #endregion
    }
}