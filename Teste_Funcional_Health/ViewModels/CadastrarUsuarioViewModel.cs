using System.ComponentModel.DataAnnotations;

namespace Teste_Funcional_Health.ViewModels
{
    public class CadastrarUsuarioViewModel
    {
        [Required(ErrorMessage = "Por favor informe o nome do Usuário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor informe o sobrenome do usuário")]
        public string SobreNome { get; set; }

        [Required(ErrorMessage = "Por favor informe o seu email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Por favor informe um email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor informe sua senha")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Por favor Confirme a sua senha")]
        [Compare("PasswordHash")]
        public string ConfirmarSenha { get; set; }
    }
}
