using System;

namespace Model
{
    public abstract class Base
    {
        // model de base
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
