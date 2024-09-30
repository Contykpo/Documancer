using Documancer.Application.Features.Contacts.DTOs;
using Documancer.Application.Features.Contacts.Caching;
using Documancer.Application.Features.Contacts.Specifications;

namespace Documancer.Application.Features.Contacts.Queries.Pagination
{
    public class ContactsWithPaginationQuery : ContactAdvancedFilter, ICacheableRequest<PaginatedData<ContactDto>>
    {
        #region Properties and Fields

        public string CacheKey => ContactCacheKey.GetPaginationCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => ContactCacheKey.MemoryCacheEntryOptions;
        public ContactAdvancedSpecification Specification => new ContactAdvancedSpecification(this);

        #endregion

        #region Method

        public override string ToString()
        {
            return $"Listview:{ListView}-{LocalTimezoneOffset}, Search:{Keyword}, {OrderBy}, {SortDirection}, {PageNumber}, {PageSize}";
        }

        #endregion
    }

    public class ContactsWithPaginationQueryHandler : IRequestHandler<ContactsWithPaginationQuery, PaginatedData<ContactDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ContactsWithPaginationQueryHandler> _localizer;

        #endregion

        #region Constructors

        public ContactsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ContactsWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<ContactDto>> Handle(ContactsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Contacts.OrderBy($"{request.OrderBy} {request.SortDirection}")
                                    .ProjectToPaginatedDataAsync<Contact, ContactDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
            return data;
        }

        #endregion
    }
}