using AutoMapper;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Teste_Funcional_Health.Extensions;
using Teste_Funcional_Health.Helpers;
using Teste_Funcional_Health.ViewModels;

namespace Teste_Funcional_Health.Controllers
{
    [Route("api/usuario")]
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
                Mensagem = "O usuário foi logado com sucesso!",
                retorno = JWT
            });

        }

        [HttpPost("CadastrarUsuario")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioViewModel model)
        {

            var result = await UsuarioRepository.CadastrarUsuario(Mapper.Map<Usuario>(model));

            if (result != null)
            {
                AdicionarErros(result);
            }

            if (PossuiErros())
            {
                return GiveResponse();
            }

            var FindedUsuario = await UsuarioRepository.FindByEmail(model.Email);
            string NumeroConta = await ContaRepository.CadastrarConta(FindedUsuario.Id);

            return GiveResponse(new SucessResponseViewModel()
            {
                Mensagem = "Usuário foi cadastrado com sucesso!",
                retorno = new
                {
                    NomeUsuario = FindedUsuario.Nome + " " + FindedUsuario.SobreNome,
                    Email = FindedUsuario.Email,
                    NumeroConta
                }
            });
        }

        [HttpGet("DadosUsuarios")]
        public async Task<IActionResult> DadosUsuarios()
        {
            return GiveResponse(new SucessResponseViewModel()
            {
                Mensagem = "Dados dos usuários",
                retorno = new
                {
                    QuantidadeUsuario = UsuarioRepository.GetAllWithExpression(usuario => usuario.Status).Result.Count(),
                    Usuarios = UsuarioRepository.GetAllWithExpression(usuario => usuario.Status).Result.Select(usuario => new DadosUsuarioViewModel()
                    {
                        Nome = usuario.Nome + " "+usuario.SobreNome,
                        Email = usuario.Email,
                        NumeroConta = ContaRepository.GetWithExpression(conta=>conta.IdUsuario.Equals(usuario.Id)).Result.Numero,
                        Saldo = " R$ " + ContaRepository.GetWithExpression(conta => conta.IdUsuario.Equals(usuario.Id)).Result.Saldo
                    })
                }
            });
        }
    }
}
