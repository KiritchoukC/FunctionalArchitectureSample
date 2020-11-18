using Architecture.Infrastructure.Todo;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ITodoItemRepository, TodoItemRepository>();
            
            return services;
        }
    }
}