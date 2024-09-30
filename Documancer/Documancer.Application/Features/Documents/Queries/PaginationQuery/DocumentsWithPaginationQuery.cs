using Documancer.Application.Features.Documents.Caching;
using Documancer.Application.Features.Documents.DTOs;
using Documancer.Application.Features.Documents.Specifications;

namespace Documancer.Application.Features.Documents.Queries.PaginationQuery
{
    public class DocumentsWithPaginationQuery : AdvancedDocumentsFilter, ICacheableRequest<PaginatedData<DocumentDto>>
    {
        #region Properties and Fields

        public AdvancedDocumentsSpecification Specification => new(this);

        public string CacheKey => DocumentCacheKey.GetPaginationCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => DocumentCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"CurrentUserId:{CurrentUser?.UserId},ListView:{ListView}-{LocalTimezoneOffset},Search:{Keyword},OrderBy:{OrderBy} {SortDirection},{PageNumber},{PageSize}";
        }

        #endregion
    }

    public class DocumentsQueryHandler : IRequestHandler<DocumentsWithPaginationQuery, PaginatedData<DocumentDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public DocumentsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
        )
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<DocumentDto>> Handle(DocumentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Documents.OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectToPaginatedDataAsync<Document, DocumentDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

            return data;
        }

        #endregion
    }
}