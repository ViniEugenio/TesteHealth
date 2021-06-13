using System.ComponentModel.DataAnnotations;

namespace Teste_Funcional_Health.ViewModels
{
    public class DepositarViewModel
    {
        [Required(ErrorMessage ="Por favor informe o número da sua conta para realizar o depósito")]
        public string NumeroConta { get; set; }

        [Required(ErrorMessage ="Por favor informe o valor desejado para realizar o depósito")]
        public double Valor { get; set; }
    }
}
