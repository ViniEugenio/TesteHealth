using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Teste_Funcional_Health.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void SwaggerConfigurationServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Teste para Desenvolvedor Funcional Health",
                    Description = "Teste para Desenvolvedor Funcioanl Health",
                    Contact = new OpenApiContact() { Name = "Vinícius Fernandes Eugênio - 23", Email = "vinicius_eugenio07@hotmail.com" }
                });

            });
        }

        public static void SwaggerConfigure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teste_Funcional_Health v1"));
        }
    }
}
