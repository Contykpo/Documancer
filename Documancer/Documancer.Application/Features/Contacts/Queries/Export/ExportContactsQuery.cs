﻿using Documancer.Application.Features.Contacts.DTOs;
using Documancer.Application.Features.Contacts.Specifications;
using Documancer.Application.Features.Contacts.Queries.Pagination;

namespace Documancer.Application.Features.Contacts.Queries.Export
{
    public class ExportContactsQuery : ContactAdvancedFilter, IRequest<Result<byte[]>>
    {
        public ContactAdvancedSpecification Specification => new ContactAdvancedSpecification(this);
    }

    public class ExportContactsQueryHandler : IRequestHandler<ExportContactsQuery, Result<byte[]>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportContactsQueryHandler> _localizer;
        private readonly ContactDto _dto = new();

        #endregion

        #region Constructors

        public ExportContactsQueryHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ExportContactsQueryHandler> localizer
        )
        {
            _context = context;
            _mapper = mapper;
            _excelService = excelService;
            _localizer = localizer;
        }

        #endregion

        #region Methods

#nullable disable warnings
        public async Task<Result<byte[]>> Handle(ExportContactsQuery request, CancellationToken cancellationToken)
        {
            var data = await _context.Contacts.ApplySpecification(request.Specification)
                        .OrderBy($"{request.OrderBy} {request.SortDirection}")
                        .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            var result = await _excelService.ExportAsync(data,
                new Dictionary<string, Func<ContactDto, object?>>()
                {
                // TODO: Define the fields that should be exported, for example:
                {_localizer[_dto.GetMemberDescription(x=>x.Name)],item => item.Name},
                {_localizer[_dto.GetMemberDescription(x=>x.Description)],item => item.Description},
                {_localizer[_dto.GetMemberDescription(x=>x.Email)],item => item.Email},
                {_localizer[_dto.GetMemberDescription(x=>x.PhoneNumber)],item => item.PhoneNumber},
                {_localizer[_dto.GetMemberDescription(x=>x.Country)],item => item.Country}
                },
                _localizer[_dto.GetClassDescription()]);

            return await Result<byte[]>.SuccessAsync(result);
        }

        #endregion
    }
}