using Documancer.Application.Features.KeyValues.Caching;
using Documancer.Application.Features.KeyValues.DTOs;
using Documancer.Application.Features.KeyValues.Specifications;

namespace Documancer.Application.Features.KeyValues.Queries.PaginationQuery
{
    public class KeyValuesWithPaginationQuery : KeyValueAdvancedFilter, ICacheableRequest<PaginatedData<KeyValueDto>>
    {
        #region Properties and Fields

        public KeyValueAdvancedSpecification Specification => new(this);

        public string CacheKey => $"{nameof(KeyValuesWithPaginationQuery)},{this}";
        public MemoryCacheEntryOptions? Options => KeyValueCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Picklist:{Picklist},Search:{Keyword},OrderBy:{OrderBy} {SortDirection},{PageNumber},{PageSize}";
        }

        #endregion
    }

    public class KeyValuesQueryHandler : IRequestHandler<KeyValuesWithPaginationQuery, PaginatedData<KeyValueDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public KeyValuesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<KeyValueDto>> Handle(KeyValuesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.KeyValues.OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectToPaginatedDataAsync<KeyValue, KeyValueDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

            return data;
        }

        #endregion
    }
}