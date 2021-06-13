using Data.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Teste_Funcional_Health.Extensions;
using Teste_Funcional_Health.ViewModels;

namespace Teste_Funcional_Health.Helpers
{
    public static class JWTHelper
    {
        public static LoginResponseViewModel GerarJwt(Usuario Usuario, JWTSettings JWTSettings)
        {
            var encodedToken = CodificarToken(JWTSettings);
            return ObterRespostaToken(encodedToken, Usuario, JWTSettings);
        }

        public static string CodificarToken(JWTSettings JWTSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = JWTSettings.Emissor,
                Audience = JWTSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(JWTSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        public static LoginResponseViewModel ObterRespostaToken(string encodedToken, Usuario user, JWTSettings JWTSettings)
        {
            return new LoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(JWTSettings.ExpiracaoHoras).TotalSeconds,
            };
        }
    }
}
