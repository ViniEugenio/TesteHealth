using Microsoft.AspNetCore.Identity;
using System;

namespace Data.Models
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime DataAlteracao { get; set; } = DateTime.UtcNow;
        public bool Status { get; set; } = true;

        //Navigation Properties
        public Conta Conta { get; set; }
    }
}
