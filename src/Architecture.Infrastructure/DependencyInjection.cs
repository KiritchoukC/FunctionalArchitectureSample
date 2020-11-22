
namespace Architecture.Infrastructure
{
    using Architecture.Infrastructure.Todo;

    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<ITodoItemRepository, TodoItemRepository>();

            return services;
        }
    }
}