using Architecture.DataSource.MongoDb.Todo;
using Architecture.Infrastructure.Todo;

using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Architecture.DataSource.MongoDb
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString, string database)
        {
            var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
            mongoSettings.ApplicationName = "TodoFunctionalApp";

            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoSettings));

            services.AddTransient<ITodoItemDataSource, TodoItemDataSource>();
            
            return services;
        }
    }
}