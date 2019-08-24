using System;
using System.Collections.Generic;

namespace Model
{
    public class Cadastro
    {
        public Guid PautaId { get; set; }
        public List<Eleitor> Eleitores { get; set; }
    }
}
