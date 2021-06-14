using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Teste_Funcional_Health;

namespace TestesIntegracao
{
    public abstract class BaseTest
    {
        public HttpClient Cliente;

        public BaseTest()
        {
            var server = new WebApplicationFactory<Startup>();
            Cliente = server.CreateClient();
        }
    }
}
