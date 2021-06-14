using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestesIntegracao.Helpers;
using Xunit;

namespace TestesIntegracao
{
    public class TransacoesTestes : BaseTest
    {

        private readonly string UsuarioCadastrado;
        private readonly string ContaNaoEncontrada;
        private readonly string DepositoRealizado;
        private readonly string SaqueRealizado;
        private readonly string SaldoRecuperado;

        public TransacoesTestes()
        {
            UsuarioCadastrado = "Usuário foi cadastrado com sucesso!";
            ContaNaoEncontrada = "Número da conta não foi encontrado por favor tente novamente mais tarde";
            DepositoRealizado = "Depósito realizado com sucesso!";
            SaqueRealizado = "Saque realizado com sucesso!";
            SaldoRecuperado = "Saldo recuperado com sucesso!";
        }

        [Theory]
        [InlineData("NumeroContaErrado", 100)]
        public async Task TesteSacarNaoEncontrandoConta(string NumeroConta, double Valor)
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Adicionando Token de Acesso
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiResponse.retorno.accessToken);

                model = JsonConvert.SerializeObject(new
                {
                    NumeroConta,
                    Valor
                });

                Body = new StringContent(model, Encoding.UTF8, "application/json");

                //Act
                using (HttpResponseMessage response2 = await Cliente.PostAsync("api/transacoes/Sacar", Body))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Contains(ContaNaoEncontrada, result);
                }
            }
        }

        [Theory]
        [InlineData("NumeroContaErrado", 100)]
        public async Task TesteDepositarNaoEncontrandoConta(string NumeroConta, double Valor)
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Adicionando Token de Acesso
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiResponse.retorno.accessToken);

                model = JsonConvert.SerializeObject(new
                {
                    NumeroConta,
                    Valor
                });

                Body = new StringContent(model, Encoding.UTF8, "application/json");

                //Act
                using (HttpResponseMessage response2 = await Cliente.PostAsync("api/transacoes/Depositar", Body))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Contains(ContaNaoEncontrada, result);
                }
            }
        }

        [Fact]
        public async Task TesteSaldoNaoEncontrandoConta()
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Adicionando Token de Acesso
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiResponse.retorno.accessToken);

                //Act
                using (HttpResponseMessage response2 = await Cliente.GetAsync("api/transacoes/Saldo?NumeroConta=123"))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Contains(ContaNaoEncontrada, result);
                }
            }
        }

        [Theory]
        [InlineData("NumeroContaErrado", 100)]
        public async Task TesteSacarNaoPassandoJWT(string NumeroConta, double Valor)
        {
            var model = JsonConvert.SerializeObject(new
            {
                NumeroConta,
                Valor
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response2 = await Cliente.PostAsync("api/transacoes/Sacar", Body))
            {

                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response2.StatusCode);
            }
        }

        [Theory]
        [InlineData("NumeroContaErrado", 100)]
        public async Task TesteDepositarNaoPassandoJWT(string NumeroConta, double Valor)
        {
            var model = JsonConvert.SerializeObject(new
            {
                NumeroConta,
                Valor
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response2 = await Cliente.PostAsync("api/transacoes/Depositar", Body))
            {

                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response2.StatusCode);
            }
        }

        [Fact]
        public async Task TesteSaldoNaoPassandoJWT()
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Act
                using (HttpResponseMessage response2 = await Cliente.GetAsync("api/transacoes/Saldo?NumeroConta=123"))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Equal(HttpStatusCode.Unauthorized, response2.StatusCode);
                }
            }
        }


        [Theory]
        [InlineData(100)]
        public async Task TesteRealizandoDeposito(double Valor)
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Adicionando Token de Acesso
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiResponse.retorno.accessToken);

                model = JsonConvert.SerializeObject(new
                {
                    retorno.NumeroConta,
                    Valor
                });

                Body = new StringContent(model, Encoding.UTF8, "application/json");

                //Act
                using (HttpResponseMessage response2 = await Cliente.PostAsync("api/transacoes/Depositar", Body))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Contains(DepositoRealizado, result);
                }
            }
        }

        [Theory]
        [InlineData(100)]
        public async Task TesteRealizandoSaque(double Valor)
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Adicionando Token de Acesso
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiResponse.retorno.accessToken);

                model = JsonConvert.SerializeObject(new
                {
                    retorno.NumeroConta,
                    Valor
                });

                Body = new StringContent(model, Encoding.UTF8, "application/json");

                //Act
                using (HttpResponseMessage response2 = await Cliente.PostAsync("api/transacoes/Depositar", Body))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert do login foi dado com inválido
                    Assert.Contains(DepositoRealizado, result);
                }

                model = JsonConvert.SerializeObject(new
                {
                    retorno.NumeroConta,
                    Valor
                });

                Body = new StringContent(model, Encoding.UTF8, "application/json");

                //Act
                using (HttpResponseMessage response3 = await Cliente.PostAsync("api/transacoes/Sacar", Body))
                {
                    result = await response3.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Contains(SaqueRealizado, result);
                }
            }
        }

        [Fact]
        public async Task TesteRecuperandoSaldo()
        {
            //Arrange
            CadastrandoUsuarioHelper retorno = await CadastrarUsuario();

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(UsuarioCadastrado, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "Teste123@"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<LoginHelper>(result);

                //Adicionando Token de Acesso
                Cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiResponse.retorno.accessToken);

                //Act
                using (HttpResponseMessage response2 = await Cliente.GetAsync("api/transacoes/Saldo?NumeroConta=" + retorno.NumeroConta))
                {
                    result = await response2.Content.ReadAsStringAsync();

                    //Assert
                    Assert.Contains(SaldoRecuperado, result);
                }
            }
        }


        public async Task<CadastrandoUsuarioHelper> CadastrarUsuario()
        {
            string Nome = "Usuário";
            string SobreNome = "de Teste";
            string Email = $"{Guid.NewGuid()}@teste.com";
            string Senha = "Teste123@";
            string ConfirmarSenha = Senha;


            var model = JsonConvert.SerializeObject(new
            {
                Nome,
                SobreNome,
                Email,
                PasswordHash = Senha,
                ConfirmarSenha
            });

            HttpContent Body = new StringContent(model, Encoding.UTF8, "application/json");

            CadastrarUsuarioSucesso ApiResponse = new CadastrarUsuarioSucesso();
            bool retornoOk = false;

            while (!retornoOk)
            {
                //Act
                using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/CadastrarUsuario", Body))
                {
                    var result = await response.Content.ReadAsStringAsync();

                    if (result.Contains($"O login '{Email}' já está sendo utilizado."))
                    {
                        Email = $"{Guid.NewGuid()}@teste.com";

                        model = JsonConvert.SerializeObject(new
                        {
                            Nome,
                            SobreNome,
                            Email,
                            PasswordHash = Senha,
                            ConfirmarSenha = Senha
                        });

                        Body = new StringContent(model, Encoding.UTF8, "application/json");
                    }

                    else
                    {
                        ApiResponse = JsonConvert.DeserializeObject<CadastrarUsuarioSucesso>(result);
                        retornoOk = true;
                    }


                }
            }

            return new CadastrandoUsuarioHelper()
            {
                Email = Email,
                Mensagem = ApiResponse.Mensagem,
                NumeroConta = ApiResponse.Retorno.NumeroConta
            };
        }

    }
}
