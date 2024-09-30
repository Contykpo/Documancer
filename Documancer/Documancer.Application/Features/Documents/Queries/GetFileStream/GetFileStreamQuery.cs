using Documancer.Application.Features.Documents.Caching;

namespace Documancer.Application.Features.Documents.Queries.GetFileStream
{
    public class GetFileStreamQuery : ICacheableRequest<(string, byte[])>
    {
        #region Properties and Fields

        public int Id { get; set; }

        public string CacheKey => DocumentCacheKey.GetStreamByIdKey(Id);
        public MemoryCacheEntryOptions? Options => DocumentCacheKey.MemoryCacheEntryOptions;

        #endregion

        #region Constructors

        public GetFileStreamQuery(int id)
        {
            Id = id;
        }

        #endregion
    }

    public class GetFileStreamQueryHandler : IRequestHandler<GetFileStreamQuery, (string, byte[])>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;

        #endregion

        #region Constructors

        public GetFileStreamQueryHandler(
            IApplicationDbContext context
        )
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<(string, byte[])> Handle(GetFileStreamQuery request, CancellationToken cancellationToken)
        {
            var item = await _context.Documents.FindAsync(new object?[] { request.Id }, cancellationToken);

            if (item is null)
            {
                throw new Exception($"not found document entry by Id:{request.Id}.");
            }

            if (string.IsNullOrEmpty(item.URL))
            {
                return (string.Empty, Array.Empty<byte>());
            }

            var filepath = Path.Combine(Directory.GetCurrentDirectory(), item.URL);

            if (!File.Exists(filepath))
            {
                return (string.Empty, Array.Empty<byte>());
            }

            var fileName = new FileInfo(filepath).Name;
            var buffer = await File.ReadAllBytesAsync(filepath, cancellationToken);

            return (fileName, buffer);
        }

        #endregion

        internal class DocumentsQuery : Specification<Document>
        {
            public DocumentsQuery(string userId, string tenantId, string keyword)
            {
                Query.Where(p => (p.CreatedBy == userId && p.IsPublic == false) || p.IsPublic == true)
                    .Where(x => x.TenantId == tenantId, !string.IsNullOrEmpty(tenantId))
                    .Where(x => x.Title!.Contains(keyword) || x.Description!.Contains(keyword), !string.IsNullOrEmpty(keyword));
            }
        }
    }
}