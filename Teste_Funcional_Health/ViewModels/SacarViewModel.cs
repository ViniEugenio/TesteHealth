using System.ComponentModel.DataAnnotations;

namespace Teste_Funcional_Health.ViewModels
{
    public class SacarViewModel
    {
        [Required(ErrorMessage = "Por favor informe o número da sua conta para realizar o saque")]
        public string NumeroConta { get; set; }

        [Required(ErrorMessage = "Por favor informe o valor desejado para realizar o saque")]
        public double Valor { get; set; }
    }
}
