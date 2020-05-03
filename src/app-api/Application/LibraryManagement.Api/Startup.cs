using Autofac;
using LibraryManagement.Api.Framework.Extensions;
using LibraryManagement.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Api
{
    public class Startup
    {
        private readonly ILibraryManagementConfig _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = new LibraryManagementConfig(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistenceServices(_configuration);
            services.AddApiServices();
            services.AddJwtAuthentication(_configuration);
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterPersistenceDependencies();
            builder.RegisterApiDependencies(_configuration);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseApiRequestPipeline();
            app.UseEngineContext();
        }
    }
}
