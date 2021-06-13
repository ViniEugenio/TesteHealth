using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teste_Funcional_Health.Configuration;

namespace Teste_Funcional_Health
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.SwaggerConfigurationServices();
            services.MapperConfigureServices();
            services.IdentityConfigurationServices(Configuration);
            services.DependecyInjectionConfigureService();
            services.APIConfigureServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.SwaggerConfigure();
            app.APIConfigure(env);
        }
    }
}
