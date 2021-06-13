using AutoMapper;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Teste_Funcional_Health.ViewModels;

namespace Teste_Funcional_Health.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void MapperConfigureServices(this IServiceCollection services)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CadastrarUsuarioViewModel,Usuario>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
