using Documancer.Application.Features.Products.Caching;
using Documancer.Application.Features.Products.DTOs;
using Documancer.Application.Features.Products.Specifications;

namespace Documancer.Application.Features.Products.Queries.Pagination
{
    public class ProductsWithPaginationQuery : ProductAdvancedFilter, ICacheableRequest<PaginatedData<ProductDto>>
    {
        #region Properties and Fields

        public ProductAdvancedSpecification Specification => new(this);

        public string CacheKey => ProductCacheKey.GetPaginationCacheKey($"{this}");

        public MemoryCacheEntryOptions? Options => ProductCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Methods

        // The currently logged-in user.
        public override string ToString()
        {
            return $"CurrentUser:{CurrentUser?.UserId},ListView:{ListView}-{LocalTimezoneOffset},Search:{Keyword},Name:{Name},Brand:{Brand},Unit:{Unit},MinPrice:{MinPrice},MaxPrice:{MaxPrice},SortDirection:{SortDirection},OrderBy:{OrderBy},{PageNumber},{PageSize}";
        }

        #endregion
    }

    public class ProductsWithPaginationQueryHandler : IRequestHandler<ProductsWithPaginationQuery, PaginatedData<ProductDto>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<ProductsWithPaginationQueryHandler> _localizer;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ProductsWithPaginationQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<ProductsWithPaginationQueryHandler> localizer
        )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        #endregion

        #region Methods

        public async Task<PaginatedData<ProductDto>> Handle(ProductsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Products.OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectToPaginatedDataAsync<Product, ProductDto>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

            return data;
        }

        #endregion
    }
}