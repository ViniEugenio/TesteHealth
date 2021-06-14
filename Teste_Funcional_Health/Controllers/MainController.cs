using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Teste_Funcional_Health.ViewModels;

namespace Teste_Funcional_Health.Controllers
{
    [ApiController]
    [Route("api")]
    public abstract class MainController : Controller
    {
        private readonly List<string> Erros;

        public MainController()
        {
            Erros = new List<string>();
        }

        protected IActionResult GiveResponse(SucessResponseViewModel response = null)
        {
            if (PossuiErros())
            {
                return BadRequest(new ValidationProblemDetails(
                        new Dictionary<string, string[]>
                        {
                            {
                                "Mensagens",
                                Erros.ToArray()
                            }
                        }
                    ));
            }

            return Ok(response);
        }

        protected void AdicionarErro(string Erro)
        {
            Erros.Add(Erro);
        }

        protected void AdicionarErros(string[] Erros)
        {
            this.Erros.AddRange(Erros);
        }

        protected bool PossuiErros()
        {
            return Erros.Any();
        }
    }
}
