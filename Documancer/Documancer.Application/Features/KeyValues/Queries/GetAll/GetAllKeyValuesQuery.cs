using Documancer.Application.Features.KeyValues.Caching;
using Documancer.Application.Features.KeyValues.DTOs;

namespace Documancer.Application.Features.KeyValues.Queries.GetAll
{
    public class GetAllKeyValuesQuery : ICacheableRequest<IEnumerable<KeyValueDto>>
    {
        public string CacheKey => KeyValueCacheKey.GetAllCacheKey;

        public MemoryCacheEntryOptions? Options => KeyValueCacheKey.MemoryCacheEntryOptions;
    }

    public class GetAllKeyValuesQueryHandler : IRequestHandler<GetAllKeyValuesQuery, IEnumerable<KeyValueDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public GetAllKeyValuesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<KeyValueDto>> Handle(GetAllKeyValuesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.KeyValues.OrderBy(x => x.Name).ThenBy(x => x.Value)
                .ProjectTo<KeyValueDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return data;
        }

        #endregion
    }
}