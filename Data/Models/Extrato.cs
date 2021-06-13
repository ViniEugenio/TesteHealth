using System;

namespace Data.Models
{
    public class Extrato : BaseModel
    {
        public Guid ContaId { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
        public string Valor { get; set; }

        //Navigation Properties
        public Conta Conta { get; set; }
    }

    public enum TipoOperacao
    {
        Sacar,
        Depositar
    }
}
