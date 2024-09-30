using Documancer.Application.Common.PublishStrategies;
using Documancer.Application.Pipeline;
using Documancer.Application.Pipeline.PreProcessors;
using Microsoft.Extensions.DependencyInjection;

namespace Documancer.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(config => { config.AddMaps(Assembly.GetExecutingAssembly()); });
            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DbExceptionHandler<,,>));
            
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.NotificationPublisher = new ParallelNoWaitPublisher();
                config.AddRequestPreProcessor(typeof(IRequestPreProcessor<>), typeof(ValidationPreProcessor<>));
                config.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
                config.AddOpenBehavior(typeof(MemoryCacheBehaviour<,>));
                config.AddOpenBehavior(typeof(FusionCacheBehaviour<,>));
                config.AddOpenBehavior(typeof(CacheInvalidationBehaviour<,>));

            });

            services.AddLazyCache();
            services.AddScoped<UserProfileStateService>();
            
            return services;
        }
    }
}