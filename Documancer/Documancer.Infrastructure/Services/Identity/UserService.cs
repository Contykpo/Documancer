using AutoMapper;
using AutoMapper.QueryableExtensions;
using Documancer.Application.Features.Identity.DTOs;
using Documancer.Domain.Identity;
using ZiggyCreatures.Caching.Fusion;

namespace Documancer.Infrastructure.Services.Identity
{
    public class UserService : IUserService
    {
        #region Events

        public event Func<Task>? OnChange;

        #endregion

        #region Properties and Fields

        private const string CACHEKEY = "ALL-ApplicationUserDto";
        private readonly IFusionCache _fusionCache;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<ApplicationUserDto> DataSource { get; private set; }

        #endregion

        #region Constructors

        public UserService(
            IFusionCache fusionCache,
            IMapper mapper,
            IServiceScopeFactory scopeFactory)
        {
            _fusionCache = fusionCache;
            _mapper = mapper;

            var scope = scopeFactory.CreateScope();

            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            DataSource = new List<ApplicationUserDto>();
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            DataSource = _fusionCache.GetOrSet(CACHEKEY,
                _ => _userManager.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .ProjectTo<ApplicationUserDto>(_mapper.ConfigurationProvider).OrderBy(x => x.UserName)
                .ToList())
                ?? new List<ApplicationUserDto>();

            OnChange?.Invoke();
        }


        public void Refresh()
        {
            _fusionCache.Remove(CACHEKEY);

            DataSource = _fusionCache.GetOrSet(CACHEKEY,
                _ => _userManager.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .ProjectTo<ApplicationUserDto>(_mapper.ConfigurationProvider).OrderBy(x => x.UserName)
                .ToList())
                ?? new List<ApplicationUserDto>();

            OnChange?.Invoke();
        }

        #endregion
    }
}