namespace Architecture.Presentation
{
    using Architecture.Application;
    using Architecture.DataSource.Cache;
    using Architecture.DataSource.MongoDb;
    using Architecture.Infrastructure;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Serilog;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplication();
            services.AddInfrastructure();
            services.AddCache(Configuration.GetConnectionString("Cache.Redis"));
            services.AddDatabase(
                Configuration.GetConnectionString("MongoDb:Connection"),
                Configuration.GetConnectionString("MongoDb:Database"));
            services.AddOpenApiDocument();
            services.AddSingleton(svc => svc.GetRequiredService<ILoggerFactory>().CreateLogger("Global"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}