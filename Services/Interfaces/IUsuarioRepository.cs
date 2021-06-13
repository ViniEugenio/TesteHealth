using Data.Models;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<string[]> CadastrarUsuario(Usuario model);
        Task<string> Login(string Email, string Senha);
        Task<Usuario> FindByEmail(string Email);
    }
}
