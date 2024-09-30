using Documancer.Application.Features.Loggers.Caching;
using Documancer.Application.Features.Loggers.DTOs;
using Documancer.Application.Features.Loggers.Specifications;

namespace Documancer.Application.Features.Loggers.Queries.PaginationQuery
{
    public class LogsWithPaginationQuery : LoggerAdvancedFilter, ICacheableRequest<PaginatedData<LogDto>>
    {
        #region Properties and Fields

        public int LocalTimezoneOffset { get; set; }
        public LoggerAdvancedSpecification Specification => new(this);

        public string CacheKey => LogsCacheKey.GetPaginationCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => LogsCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"Listview:{ListView}-{LocalTimezoneOffset},{Level},Search:{Keyword},OrderBy:{OrderBy} {SortDirection},{PageNumber},{PageSize}";
        }

        #endregion
    }

    public class LogsQueryHandler : IRequestHandler<LogsWithPaginationQuery, PaginatedData<LogDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public LogsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<LogDto>> Handle(LogsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Loggers.OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectToPaginatedDataAsync<Logger, LogDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

            return data;
        }

        #endregion
    }
}