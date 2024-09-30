using AutoMapper;
using AutoMapper.QueryableExtensions;
using Documancer.Application.Common.Interfaces.MultiTenant;
using Documancer.Application.Features.Tenants.Caching;
using Documancer.Application.Features.Tenants.DTOs;
using ZiggyCreatures.Caching.Fusion;

namespace Documancer.Infrastructure.Services.MultiTenant
{
    public class TenantService : ITenantService
    {
        #region Events

        public event Func<Task>? OnChange;

        #endregion

        #region Properties and Fields

        private readonly IApplicationDbContext _context;
        private readonly IFusionCache _fusionCache;
        private readonly IMapper _mapper;

        public List<TenantDto> DataSource { get; private set; } = new();

        #endregion

        #region Constructors

        public TenantService(IFusionCache fusionCache, IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            var scope = scopeFactory.CreateScope();
        
            _context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            _fusionCache = fusionCache;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            DataSource = _fusionCache.GetOrSet(TenantCacheKey.TenantsCacheKey,
                _ => _context.Tenants.OrderBy(x => x.Name)
                    .ProjectTo<TenantDto>(_mapper.ConfigurationProvider)
                    .ToList()) ?? new List<TenantDto>();
        }

        public void Refresh()
        {
            _fusionCache.Remove(TenantCacheKey.TenantsCacheKey);
            DataSource = _fusionCache.GetOrSet(TenantCacheKey.TenantsCacheKey,
                _ => _context.Tenants.OrderBy(x => x.Name)
                    .ProjectTo<TenantDto>(_mapper.ConfigurationProvider)
                    .ToList()) ?? new List<TenantDto>();
            OnChange?.Invoke();
        }

        #endregion
    }
}