using System;

namespace Data.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; private set; }
        public DateTime DataCadastro { get; private set; } = DateTime.UtcNow;
        public DateTime DataAlteracao { get; private set; } = DateTime.UtcNow;
        public bool Status { get; private set; } = true;

        protected void Deletar()
        {
            Status = false;
        }
    }
}
