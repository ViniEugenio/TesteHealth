using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Teste_Funcional_Health;
using Xunit;

namespace TestesIntegracao
{
    public class UsuarioTestes
    {

        private readonly HttpClient Cliente;

        public UsuarioTestes()
        {
            var server = new WebApplicationFactory<Startup>();
            Cliente = server.CreateClient();
        }

        [Fact]
        public async Task CadastroUsuarioNaoPassandoDados()
        {
            //Arrange

            var model = JsonConvert.SerializeObject(new
            {
                Nome = "",
                SobreNome = "",
                Email = "",
                PasswordHash = "",
                ConfirmarSenha = ""
            });

            HttpContent Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/CadastrarUsuario", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<object>(result);
            }

            Assert.Equal(1, 2);
        }
    }
}
