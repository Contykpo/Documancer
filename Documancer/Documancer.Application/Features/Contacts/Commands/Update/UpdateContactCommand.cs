using System.ComponentModel;
using Documancer.Application.Features.Contacts.DTOs;
using Documancer.Application.Features.Contacts.Caching;

namespace Documancer.Application.Features.Contacts.Commands.Update
{
    public class UpdateContactCommand : ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        [Description("Id")]
        public int Id { get; set; }
        [Description("Name")]
        public string Name { get; set; }
        [Description("Description")]
        public string? Description { get; set; }
        [Description("Email")]
        public string? Email { get; set; }
        [Description("Phone number")]
        public string? PhoneNumber { get; set; }
        [Description("Country")]
        public string? Country { get; set; }

        public string CacheKey => ContactCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => ContactCacheKey.GetOrCreateTokenSource();

        #endregion

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<ContactDto, UpdateContactCommand>(MemberList.None);
                CreateMap<UpdateContactCommand, Contact>(MemberList.None);
            }
        }
    }

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateContactCommandHandler> _localizer;

        #endregion

        #region Constructors

        public UpdateContactCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<UpdateContactCommandHandler> localizer,
            IMapper mapper
        )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        #endregion

        #region Methods
        public async Task<Result<int>> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {

            var item = await _context.Contacts.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"Contact with id: [{request.Id}] not found.");
            item = _mapper.Map(request, item);
            // Raise an update domain event.
            item.AddDomainEvent(new ContactUpdatedEvent(item));

            await _context.SaveChangesAsync(cancellationToken);

            return await Result<int>.SuccessAsync(item.Id);
        }

        #endregion
    }
}