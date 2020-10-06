using Microsoft.Extensions.DependencyInjection;

namespace Architecture.DataSource.Cache
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCache(this IServiceCollection services, string connectionString)
        {
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = connectionString;
            });

            return services;
        }
    }
}