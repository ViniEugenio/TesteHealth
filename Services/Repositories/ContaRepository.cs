using Data;
using Data.Models;
using Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class ContaRepository : Repository<Conta>, IContaRepository
    {
        public ContaRepository(Context Context) : base(Context)
        {
        }

        public async Task<string> CadastrarConta(string UsuarioId)
        {
            Conta NovaConta = new Conta()
            {
                IdUsuario = UsuarioId,
                Numero = await GenerateConta(),
            };

            await Add(NovaConta);

            return NovaConta.Numero;
        }

        public async Task<string> GenerateConta()
        {
            Random rnd = new Random();
            string NumeroConta = rnd.Next(100000, 999999).ToString();

            var FindedConta = await VerificaConta(NumeroConta);

            while (!FindedConta)
            {
                NumeroConta = rnd.Next(100000, 999999).ToString();
                FindedConta = await VerificaConta(NumeroConta);
            }

            return NumeroConta;
        }

        public async Task<Conta> GetContaByNumero(string NumeroConta)
        {
            return await GetWithExpression(conta => conta.Numero.Equals(NumeroConta));
        }

        public async Task<Conta> RealizarDeposito(Guid IdConta, double Valor)
        {
            var FindedConta = await GetById(IdConta);
            FindedConta.Saldo = FindedConta.Saldo + Valor;

            await Update(FindedConta);

            return FindedConta;
        }

        public async Task<Conta> RealizarSaque(Guid IdConta, double Valor)
        {
            var FindedConta = await GetById(IdConta);

            FindedConta.Saldo = FindedConta.Saldo - Valor;
            await Update(FindedConta);

            return FindedConta;
        }

        public async Task<bool> VerificaConta(string Conta)
        {
            var FindedConta = await GetWithExpression(conta => conta.Numero.Equals(Conta) && conta.Status);

            if (FindedConta != null)
            {
                return false;
            }

            return true;
        }
    }
}
