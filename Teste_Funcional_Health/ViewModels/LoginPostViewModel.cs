using System.ComponentModel.DataAnnotations;

namespace Teste_Funcional_Health.ViewModels
{
    public class LoginPostViewModel
    {
        [DataType(DataType.EmailAddress, ErrorMessage = "Por favor insira um email válido")]
        [Required(ErrorMessage = "Por favor informe seu email para efetuar o login")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor informe sua senha para efetuar o login")]
        public string Senha { get; set; }
    }
}
