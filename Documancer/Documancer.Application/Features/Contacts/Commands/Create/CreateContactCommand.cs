using System.ComponentModel;
using Documancer.Application.Features.Contacts.DTOs;
using Documancer.Application.Features.Contacts.Caching;

namespace Documancer.Application.Features.Contacts.Commands.Create
{
    public class CreateContactCommand: ICacheInvalidatorRequest<Result<int>>
    {
        #region Properties and Fields

        [Description("Id")] 
        public int Id { get; set; }
        [Description("Name")]
        public string Name {get;set;} 
        [Description("Description")]
        public string? Description {get;set;} 
        [Description("Email")]
        public string? Email {get;set;} 
        [Description("Phone number")]
        public string? PhoneNumber {get;set;} 
        [Description("Country")]
        public string? Country {get;set;} 

        public string CacheKey => ContactCacheKey.GetAllCacheKey;
        public CancellationTokenSource? SharedExpiryTokenSource => ContactCacheKey.GetOrCreateTokenSource();

        #endregion

        private class Mapping : Profile
        {
            public Mapping()
            {
                 CreateMap<ContactDto,CreateContactCommand>(MemberList.None);
                 CreateMap<CreateContactCommand,Contact>(MemberList.None);
            }
        }
    }
    
    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Result<int>>
    {
        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateContactCommand> _localizer;

        #endregion

        #region Constructors

        public CreateContactCommandHandler(
            IApplicationDbContext context,
            IStringLocalizer<CreateContactCommand> localizer,
            IMapper mapper
        )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<Result<int>> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Contact>(request);
            // Raise a create domain event.
	        item.AddDomainEvent(new ContactCreatedEvent(item));
            
            _context.Contacts.Add(item);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return  await Result<int>.SuccessAsync(item.Id);
        }

        #endregion
    }
}