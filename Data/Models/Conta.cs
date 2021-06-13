using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Conta : BaseModel
    {
        public string IdUsuario { get; set; }
        public string Numero { get; set; }
        public double Saldo { get; set; } = 0;

        //Navigation Properties
        public Usuario Usuario { get; set; }
        public IEnumerable<Extrato> Extratos { get; set; }
    }
}
