using Documancer.Application.Features.Contacts.DTOs;
using Documancer.Application.Features.Contacts.Caching;
using Documancer.Application.Features.Contacts.Specifications;

namespace Documancer.Application.Features.Contacts.Queries.GetById
{
    public class GetContactByIdQuery : ICacheableRequest<ContactDto>
    {
        public required int Id { get; set; }
        public string CacheKey => ContactCacheKey.GetByIdCacheKey($"{Id}");
        public MemoryCacheEntryOptions? Options => ContactCacheKey.MemoryCacheEntryOptions;
    }

    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<GetContactByIdQueryHandler> _localizer;

        #endregion

        #region Constructors

        public GetContactByIdQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<GetContactByIdQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        public async Task<ContactDto> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Contacts.ApplySpecification(new ContactByIdSpecification(request.Id))
                         .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                         .FirstAsync(cancellationToken) ?? throw new NotFoundException($"Contact with id: [{request.Id}] not found.");
            return data;
        }

        #endregion
    }
}