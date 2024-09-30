using Documancer.Application.Features.Contacts.DTOs;
using Documancer.Application.Features.Contacts.Caching;

namespace Documancer.Application.Features.Contacts.Queries.GetAll
{
    public class GetAllContactsQuery : ICacheableRequest<IEnumerable<ContactDto>>
    {
        public string CacheKey => ContactCacheKey.GetAllCacheKey;
        public MemoryCacheEntryOptions? Options => ContactCacheKey.MemoryCacheEntryOptions;
    }

    public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetAllContactsQueryHandler> _localizer;

        #endregion

        #region Constructors

        public GetAllContactsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetAllContactsQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Contacts
                         .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                         .AsNoTracking()
                         .ToListAsync(cancellationToken);
            return data;
        }

        #endregion
    }
}