using Documancer.Application.Features.KeyValues.DTOs;

namespace Documancer.Application.Features.KeyValues.Queries.Export
{
    public class ExportKeyValuesQuery : IRequest<byte[]>
    {
        public string? Keyword { get; set; }
        public string OrderBy { get; set; } = "Id";
        public string SortDirection { get; set; } = "desc";
    }

    public class ExportKeyValuesQueryHandler : IRequestHandler<ExportKeyValuesQuery, byte[]>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportKeyValuesQueryHandler> _localizer;
        private readonly IMapper _mapper;

        #endregion

        #region Constructors

        public ExportKeyValuesQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportKeyValuesQueryHandler> localizer
        )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        #endregion

        #region Methods

#pragma warning disable CS8602
#pragma warning disable CS8604
        public async Task<byte[]> Handle(ExportKeyValuesQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.KeyValues.Where(x => x.Description.Contains(request.Keyword) || x.Value.Contains(request.Keyword) || x.Text.Contains(request.Keyword))
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<KeyValueDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<KeyValueDto, object?>>
                {
                //{ _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Value"], item => item.Value },
                { _localizer["Text"], item => item.Text },
                { _localizer["Description"], item => item.Description }
                },
                _localizer["Data"]
            );

            return result;
        }

        #endregion
    }
}