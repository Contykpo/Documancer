using Documancer.Application.Features.Tenants.Caching;
using Documancer.Application.Features.Tenants.DTOs;

namespace Documancer.Application.Features.Tenants.Queries.Pagination
{
    public class TenantsWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<TenantDto>>
    {
        public TenantsPaginationSpecification Specification => new(this);
        public string CacheKey => TenantCacheKey.GetPaginationCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => TenantCacheKey.MemoryCacheEntryOptions;

        public override string ToString()
        {
            return $"Search:{Keyword},OrderBy:{OrderBy} {SortDirection},{PageNumber},{PageSize}";
        }
    }

    public class TenantsWithPaginationQueryHandler : IRequestHandler<TenantsWithPaginationQuery, PaginatedData<TenantDto>>
    {
        #region Properties and Methods

        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<TenantsWithPaginationQueryHandler> _localizer;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public TenantsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<TenantsWithPaginationQueryHandler> localizer
        )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<TenantDto>> Handle(TenantsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Tenants.OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectToPaginatedDataAsync<Tenant, TenantDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

            return data;
        }

        #endregion
    }

#nullable disable warnings
    public class TenantsPaginationSpecification : Specification<Tenant>
    {
        public TenantsPaginationSpecification(TenantsWithPaginationQuery query)
        {
            Query.Where(q => q.Name != null).Where(q => q.Name.Contains(query.Keyword) || q.Description.Contains(query.Keyword), !string.IsNullOrEmpty(query.Keyword));
        }
    }
}