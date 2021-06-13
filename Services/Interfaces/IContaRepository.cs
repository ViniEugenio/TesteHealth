using Data.Models;
using System;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IContaRepository : IRepository<Conta>
    {
        Task<string> GenerateConta();
        Task<string> CadastrarConta(string UsuarioId);
        Task<bool> VerificaConta(string Conta);
        Task<Conta> GetContaByNumero(string NumeroConta);
        Task<Conta> RealizarSaque(Guid IdConta, double Valor);
        Task<Conta> RealizarDeposito(Guid IdConta, double Valor);
    }
}
