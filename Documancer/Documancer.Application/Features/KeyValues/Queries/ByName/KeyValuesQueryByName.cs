using Documancer.Application.Features.KeyValues.Caching;
using Documancer.Application.Features.KeyValues.DTOs;

namespace Documancer.Application.Features.KeyValues.Queries.ByName
{
    public class KeyValuesQueryByName : ICacheableRequest<IEnumerable<KeyValueDto>>
    {
        #region Properties and Fields

        public Picklist Name { get; set; }

        public string CacheKey => KeyValueCacheKey.GetCacheKey(Name.ToString());

        public MemoryCacheEntryOptions? Options => KeyValueCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Constructors

        public KeyValuesQueryByName(Picklist name)
        {
            Name = name;
        }

        #endregion
    }

    public class KeyValuesQueryByNameHandler : IRequestHandler<KeyValuesQueryByName, IEnumerable<KeyValueDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public KeyValuesQueryByNameHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<KeyValueDto>> Handle(KeyValuesQueryByName request, CancellationToken cancellationToken)
        {
            var data = await _context.KeyValues.Where(x => x.Name == request.Name)
                .OrderBy(x => x.Text)
                .ProjectTo<KeyValueDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return data;
        }

        #endregion
    }
}