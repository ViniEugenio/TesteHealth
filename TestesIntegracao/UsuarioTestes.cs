using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Teste_Funcional_Health;
using TestesIntegracao.Helpers;
using Xunit;

namespace TestesIntegracao
{
    public class UsuarioTestes : BaseTest
    {
        private readonly CadastrarUsuarioErros ExpectedResult1;
        private readonly string ExpectedResult2;
        private readonly string ExpectedResult3;
        private readonly string ExpectedResult4;

        public UsuarioTestes()
        {
            #region Mockando Dados

            ExpectedResult1 = new CadastrarUsuarioErros()
            {
                Nome = new List<string>() {
                    "Por favor informe o nome do Usuário",
                },
                ConfirmarSenha = new List<string>() {
                    "Por favor Confirme a sua senha",
                },
                Email = new List<string>() {
                    "Por favor informe o seu email",
                },
                PasswordHash = new List<string>() {
                    "Por favor informe sua senha",
                },
                SobreNome = new List<string>() {
                    "Por favor informe o sobrenome do usuário",
                },
            };

            ExpectedResult2 = "Usuário foi cadastrado com sucesso!";
            ExpectedResult3 = "As senhas devem conter ao menos um caracter não alfanumérico.";
            ExpectedResult4 = "O usuário foi logado com sucesso!";

            #endregion
        }

        [Theory]
        [InlineData("Vinícius", "Fernandes Eugênio", "Teste123@")]
        public async Task TesteLoginUsuarioNaoEncontrandoUsuario(string Nome, string SobreNome, string PasswordHash)
        {
            //Arrange

            CadastrandoUsuarioHelper retorno = await CadastrarUsuario(Nome, SobreNome, PasswordHash, Guid.NewGuid());

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(ExpectedResult2, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "SenhaErrada123"
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();

                //Assert do login foi dado com inválido
                Assert.Contains("Senha incorreta", result);
            }
        }

        [Theory]
        [InlineData("Vinícius", "Fernandes Eugênio", "Teste123@")]
        public async Task TesteLogandoUsuario(string Nome, string SobreNome, string PasswordHash)
        {
            //Arrange

            CadastrandoUsuarioHelper retorno = await CadastrarUsuario(Nome, SobreNome, PasswordHash, Guid.NewGuid());

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(ExpectedResult2, retorno.Mensagem);

            var model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = "SenhaErrada123"
            });

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(ExpectedResult2, retorno.Mensagem);

            model = JsonConvert.SerializeObject(new
            {
                Email = retorno.Email,
                Senha = PasswordHash
            });

            var Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/Login", Body))
            {
                var result = await response.Content.ReadAsStringAsync();
                Assert.Contains(ExpectedResult4, result);
            }
        }

        [Theory]
        [InlineData("Vinícius", "Fernandes Eugênio", "Teste123@")]
        public async Task TesteCadastrandoUsuario(string Nome, string SobreNome, string PasswordHash)
        {
            //Arrange

            CadastrandoUsuarioHelper retorno = await CadastrarUsuario(Nome, SobreNome, PasswordHash, Guid.NewGuid());

            //Assert vendo se um novo usuário foi cadastrado
            Assert.Equal(ExpectedResult2, retorno.Mensagem);

            //Assert        
            Assert.Equal(ExpectedResult2, retorno.Mensagem);
        }

        [Fact]
        public async Task TesteCadastroUsuarioNaoPassandoDados()
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
                var ApiResponse = JsonConvert.DeserializeObject<ApiErrorResponse<CadastrarUsuarioErros>>(result);

                //Assert
                ExpectedResult1.Should().BeEquivalentTo(ApiResponse.Errors);
            }

        }

        [Theory]
        [InlineData("Vinícius", "Fernandes Eugênio", "teste@teste.com", "Teste123")]
        public async Task TesteCadastroUsuarioPassandoSenhaInvalida(string Nome, string SobreNome, string Email, string PasswordHash)
        {
            //Arrange            

            var model = JsonConvert.SerializeObject(new
            {
                Nome,
                SobreNome,
                Email,
                PasswordHash,
                ConfirmarSenha = PasswordHash
            });

            HttpContent Body = new StringContent(model, Encoding.UTF8, "application/json");

            //Act
            using (HttpResponseMessage response = await Cliente.PostAsync("api/usuario/CadastrarUsuario", Body))
            {
                var result = await response.Content.ReadAsStringAsync();

                //Assert   
                Assert.Contains(ExpectedResult3, result);
            }
        }

        public async Task<CadastrandoUsuarioHelper> CadastrarUsuario(string Nome, string SobreNome, string PasswordHash, Guid id)
        {
            string Email = $"{id}@teste.com";

            var model = JsonConvert.SerializeObject(new
            {
                Nome,
                SobreNome,
                Email,
                PasswordHash,
                ConfirmarSenha = PasswordHash
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
                        Email = $"{id}@teste.com";

                        model = JsonConvert.SerializeObject(new
                        {
                            Nome,
                            SobreNome,
                            Email,
                            PasswordHash,
                            ConfirmarSenha = PasswordHash
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
                Mensagem = ApiResponse.Mensagem
            };
        }

    }
}