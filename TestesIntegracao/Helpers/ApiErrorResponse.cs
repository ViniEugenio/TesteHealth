using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestesIntegracao.Helpers
{
    public class ApiErrorResponse<T>
    {
        [JsonProperty("Errors")]
        public T Errors { get; set; }
    }

    public class CadastrarUsuarioErros
    {
        public List<string> Nome { get; set; }
        public List<string> SobreNome { get; set; }
        public List<string> Email { get; set; }
        public List<string> PasswordHash { get; set; }
        public List<string> ConfirmarSenha { get; set; }
    }

    public class CadastrarUsuarioSucesso
    {
        public string Mensagem { get; set; }
        public RetornoSucesso Retorno { get; set; }
    }

    public class RetornoSucesso
    {
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string NumeroConta { get; set; }
    }
}