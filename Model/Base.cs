using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public abstract class Base
    {
        //Classe base para propriedades em comum
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
