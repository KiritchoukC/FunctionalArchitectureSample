using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Architecture.DataSource.MongoDb
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString, string database)
        {
            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(connectionString));
            
            return services;
        }
    }
}