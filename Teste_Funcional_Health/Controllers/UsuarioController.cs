using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using System.Threading.Tasks;
using Teste_Funcional_Health.Extensions;
using Teste_Funcional_Health.Helpers;
using Teste_Funcional_Health.ViewModels;

namespace Teste_Funcional_Health.Controllers
{
    [Route("usuario")]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioRepository UsuarioRepository;
        private readonly IContaRepository ContaRepository;
        private readonly IMapper Mapper;
        private readonly JWTSettings JWTSettings;

        public UsuarioController(IUsuarioRepository UsuarioRepository, IContaRepository ContaRepository, IMapper Mapper, IOptions<JWTSettings> JWTSettings)
        {
            this.UsuarioRepository = UsuarioRepository;
            this.ContaRepository = ContaRepository;
            this.Mapper = Mapper;
            this.JWTSettings = JWTSettings.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginPostViewModel model)
        {
            var result = await UsuarioRepository.Login(model.Email, model.Senha);

            if (!string.IsNullOrEmpty(result))
            {
                AdicionarErro(result);
            }

            if (PossuiErros())
            {
                return GiveResponse();
            }


            var JWT = JWTHelper.GerarJwt(await UsuarioRepository.FindByEmail(model.Email), JWTSettings);

            return GiveResponse(new SucessResponseViewModel()
            {
                Menssagem = "O usuário foi logado com sucesso!",
                retorno = JWT
            });

        }

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario([FromBody]CadastrarUsuarioViewModel model)
        {

            var FindedUsuario = await UsuarioRepository.FindByEmail(model.UserName);

            if (FindedUsuario != null)
            {
                AdicionarErro("O email informado já está sendo usado por outro usuário, por favor informe outro email e tente novamente");
            }

            var result = await UsuarioRepository.CadastrarUsuario(Mapper.Map<Usuario>(model));

            if (result.Length > 0)
            {
                AdicionarErros(result);
            }

            if (PossuiErros())
            {
                return GiveResponse();
            }

            FindedUsuario = await UsuarioRepository.FindByEmail(model.UserName);
            string NumeroConta = await ContaRepository.CadastrarConta(FindedUsuario.Id);

            return GiveResponse(new SucessResponseViewModel()
            {
                Menssagem = "Usuário foi cadastrado com sucesso!",
                retorno = new
                {
                    NomeUsuario = FindedUsuario.Nome + " " + FindedUsuario.SobreNome,
                    Email = FindedUsuario.Email,
                    NumeroConta
                }
            });
        }
    }
}
