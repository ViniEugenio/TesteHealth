using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly UserManager<Usuario> UserManager;
        private readonly SignInManager<Usuario> SignInManager;

        public UsuarioRepository(Context Context, UserManager<Usuario> UserManager, SignInManager<Usuario> SignInManager) : base(Context)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
        }

        public async Task<string[]> CadastrarUsuario(Usuario model)
        {
            model.UserName = model.Email;

            var result = await UserManager.CreateAsync(model, model.PasswordHash);

            if (!result.Succeeded)
            {
                return result
                    .Errors
                    .Select(erro => erro.Description)
                    .ToArray();
            }

            return null;
        }

        public async Task<Usuario> FindByEmail(string Email)
        {
            return await UserManager.FindByEmailAsync(Email);
        }

        public async Task<string> Login(string Email, string Senha)
        {
            var FindedUser = await UserManager.FindByEmailAsync(Email);

            if (FindedUser == null)
            {
                return "O usuário informado não foi encontrado!";
            }

            var result = await SignInManager.PasswordSignInAsync(FindedUser, Senha, false, false);

            if (result.IsNotAllowed)
            {
                return "Este usuário ainda não está autorizado a acessar a plataforma";
            }

            else if (result.IsLockedOut)
            {
                return "Este usuário está bloqueado do sistema";
            }

            else if (!result.Succeeded)
            {
                return "Senha incorreta";
            }

            return null;
        }
    }
}
