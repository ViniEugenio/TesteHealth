using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Threading.Tasks;
using Teste_Funcional_Health.ViewModels;

namespace Teste_Funcional_Health.Controllers
{
    public class TransacoesController : MainController
    {
        private readonly IContaRepository ContaRepository;
        private readonly IExtratoRepository ExtratoRepository;

        public TransacoesController(IContaRepository ContaRepository, IExtratoRepository ExtratoRepository)
        {
            this.ContaRepository = ContaRepository;
            this.ExtratoRepository = ExtratoRepository;
        }

        [HttpPost("Sacar")]
        public async Task<IActionResult> Sacar([FromBody] SacarViewModel model)
        {
            var FindedConta = await ContaRepository.GetContaByNumero(model.NumeroConta);

            if (FindedConta == null)
            {
                AdicionarErro("Número da conta não foi encontrado por favor tente novamente mais tarde");
            }

            else if (model.Valor <= 0)
            {
                AdicionarErro("Por favor informe um valor válido para fazer o saque");
            }

            else if (FindedConta.Saldo < model.Valor)
            {
                AdicionarErro($"Saldo Indisponível para o valor informado, Saldo Atual é de R$ {FindedConta.Saldo}");
            }

            if (PossuiErros())
            {
                return GiveResponse();
            }

            var UpdatedConta = await ContaRepository.RealizarSaque(FindedConta.Id, model.Valor);

            await ExtratoRepository.Add(new Extrato()
            {
                ContaId = FindedConta.Id,
                TipoOperacao = TipoOperacao.Sacar,
                Valor = model.Valor.ToString()
            });

            return GiveResponse(new SucessResponseViewModel()
            {
                Menssagem = "Saque realizado com sucesso!",
                retorno = new
                {
                    ValorSaque = model.Valor,
                    SaldoAtual = UpdatedConta.Saldo
                }
            });
        }

        [HttpPost("Depositar")]
        public async Task<IActionResult> Depositar([FromBody] DepositarViewModel model)
        {
            var FindedConta = await ContaRepository.GetContaByNumero(model.NumeroConta);

            if (FindedConta == null)
            {
                AdicionarErro("Número da conta não foi encontrado por favor tente novamente mais tarde");
            }

            else if (model.Valor <= 0)
            {
                AdicionarErro("Por favor informe um valor válido para fazer o saque");
            }

            if (PossuiErros())
            {
                return GiveResponse();
            }

            var UpdatedConta = await ContaRepository.RealizarDeposito(FindedConta.Id, model.Valor);

            await ExtratoRepository.Add(new Extrato()
            {
                ContaId = FindedConta.Id,
                TipoOperacao = TipoOperacao.Depositar,
                Valor = model.Valor.ToString()
            });

            return GiveResponse(new SucessResponseViewModel()
            {
                Menssagem = "Depósito realizado com sucesso!",
                retorno = new
                {
                    ValorDeposito = model.Valor,
                    SaldoAtual = UpdatedConta.Saldo
                }
            });

        }

        [HttpGet("Saldo")]
        public async Task<IActionResult> Saldo(string NumeroConta)
        {
            var FindedConta = await ContaRepository.GetContaByNumero(NumeroConta);

            if (FindedConta == null)
            {
                AdicionarErro("Número da conta não foi encontrado por favor tente novamente mais tarde");
            }

            if (PossuiErros())
            {
                return GiveResponse();
            }

            return GiveResponse(new SucessResponseViewModel()
            {
                Menssagem = "Saldo recuperado com sucesso!",
                retorno = new
                {
                    SaldoDisponivel = FindedConta.Saldo
                }
            });
        }
    }
}
