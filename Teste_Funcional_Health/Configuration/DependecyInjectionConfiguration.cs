using Data;
using Microsoft.Extensions.DependencyInjection;
using Services.Repositories;
using Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Data.Models;

namespace Teste_Funcional_Health.Configuration
{
    public static class DependecyInjectionConfiguration
    {
        public static void DependecyInjectionConfigureService(this IServiceCollection services)
        {
            //Classe de Contexto
            services.AddDbContext<Context>();

            //Serviços
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IContaRepository, ContaRepository>();
            services.AddTransient<IExtratoRepository, ExtratoRepository>();
        }
    }
}
